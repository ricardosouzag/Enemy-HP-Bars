using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnemyHPBar
{
    public class EnemyHPBar : Mod, IGlobalSettings<Settings>,ICustomMenuMod
    {
        private const string version = "2.2.0";

        public static EnemyHPBar instance;

        public static GameObject canvas;
        public static GameObject bossCanvas;
        private static GameObject spriteLoader;

        public const string HPBAR_BG = "bg.png";
        public const string HPBAR_FG = "fg.png";
        public const string HPBAR_MG = "mg.png";
        public const string HPBAR_OL = "ol.png";
        public const string HPBAR_BOSSOL = "bossol.png";
        public const string HPBAR_BOSSFG = "bossfg.png";
        public const string HPBAR_BOSSBG = "bossbg.png";
        public const string SPRITE_FOLDER = "CustomHPBar";

        public static readonly string DATA_DIR = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),SPRITE_FOLDER);

        public static Sprite bg;
        public static Sprite mg;
        public static Sprite fg;
        public static Sprite ol;
        public static Sprite bossbg;
        public static Sprite bossfg;
        public static Sprite bossol;
        public bool ToggleButtonInsideMenu { get; } = true;
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggle)
        {
            return BetterMenu.GetMenu (modListMenu, toggle);
        }
        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");

            instance = this;
            Log(DATA_DIR);
            if (!Directory.Exists(DATA_DIR))
            {
                Directory.CreateDirectory(DATA_DIR);
            }
            if (!Directory.Exists(Path.Combine(DATA_DIR, "Default")))
            {
                Directory.CreateDirectory(Path.Combine(DATA_DIR, "Default"));
            }
            foreach (string res in Assembly.GetExecutingAssembly().GetManifestResourceNames().Where(t => t.EndsWith("png")))
            {
                string properRes = res.Replace("src.Resources.", "");
                string resPath = Path.Combine(DATA_DIR, "Default", properRes);
                if (File.Exists(resPath)) continue;
                
                var file = File.Create(resPath);
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(res))
                {
                    byte[] sidwhy = new byte[stream.Length];
                    stream.Read(sidwhy, 0, sidwhy.Length);
                    file.Write(sidwhy,0,sidwhy.Length);
                    file.Dispose();
                }
            }
            GetSkinList();
            LoadLoader();

            ModHooks.OnEnableEnemyHook += Instance_OnEnableEnemyHook;
            ModHooks.OnReceiveDeathEventHook += Instance_OnReceiveDeathEventHook;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            
            
            canvas = CanvasUtil.CreateCanvas(RenderMode.WorldSpace, new Vector2(1920f, 1080f));
            bossCanvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1920f, 1080f));
            canvas.GetComponent<Canvas>().sortingOrder = 1;
            bossCanvas.GetComponent<Canvas>().sortingOrder = 1;
            
            
            bossol = HPBarCreateSprite(ResourceLoader.GetBossOutlineImage());
            bossbg = HPBarCreateSprite(ResourceLoader.GetBossBackgroundImage());
            bossfg = HPBarCreateSprite(ResourceLoader.GetBossForegroundImage());
            ol = HPBarCreateSprite(ResourceLoader.GetOutlineImage());
            fg = HPBarCreateSprite(ResourceLoader.GetForegroundImage());
            mg = HPBarCreateSprite(ResourceLoader.GetMiddlegroundImage());
            bg = HPBarCreateSprite(ResourceLoader.GetBackgroundImage());

            Object.DontDestroyOnLoad(canvas);
            Object.DontDestroyOnLoad(bossCanvas);

            Log("Initialized EnemyHPBars");
        }
      
        internal Settings globalSettings = new();
        
        public void OnLoadGlobal(Settings s) => globalSettings = s;

        public Settings OnSaveGlobal() => globalSettings;

        public void LoadLoader()
        {
            if (spriteLoader == null)
            {
                spriteLoader = new GameObject();
                spriteLoader.AddComponent<ResourceLoader>();
            }
        }

        public Sprite HPBarCreateSprite(byte[] data)
        {
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(data);
            texture2D.anisoLevel = 0;
            return Sprite.Create(texture2D, new Rect((float) 0, (float) 0, (float) texture2D.width, (float) texture2D.height), Vector2.zero);
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            ActiveBosses = new List<string>();
        }

        private void Instance_OnReceiveDeathEventHook(EnemyDeathEffects enemyDeathEffects, bool eventAlreadyRecieved, 
        ref float? attackDirection, ref bool resetDeathEvent, ref bool spellBurn, ref bool isWatery)
        {
            Log($@"Enemy {enemyDeathEffects.gameObject.name} ded");
            if (enemyDeathEffects.gameObject.GetComponent<HPBar>() != null)
            {
                if (enemyDeathEffects.gameObject.GetComponent<HPBar>().oldHP == 0)
                {
                    GameObject placeHolder = new GameObject();
                    placeHolder.transform.position = enemyDeathEffects.gameObject.transform.position;
                    HealthManager phhm = placeHolder.AddComponent<HealthManager>();
                    HPBar phhp = placeHolder.AddComponent<HPBar>();
                    phhp.currHP = 18;
                    phhp.maxHP = 18;
                    phhm.hp = 0;
                }
                else
                {
                    GameObject placeHolder = new GameObject();
                    placeHolder.transform.position = enemyDeathEffects.gameObject.transform.position;
                    HealthManager phhm = placeHolder.AddComponent<HealthManager>();
                    HPBar phhp = placeHolder.AddComponent<HPBar>();
                    placeHolder.AddComponent<EnemyDeathEffects>();
                    phhp.currHP = enemyDeathEffects.gameObject.GetComponent<HPBar>().oldHP;
                    phhp.maxHP = enemyDeathEffects.gameObject.GetComponent<HPBar>().maxHP;
                    phhm.hp = 0;
                }
            }

            return;
        }

        private bool Instance_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            HealthManager hm = enemy.GetComponent<HealthManager>();

            if (hm == null) return false;
            
            EnemyDeathEffects ede = enemy.GetComponent<EnemyDeathEffects>();
            EnemyDeathTypes? deathType = ede == null 
                ? null 
                : DEATH_FI?.GetValue(ede) as EnemyDeathTypes?;

            bool isBoss = hm.hp >= 200 || deathType == EnemyDeathTypes.LargeInfected;
            
            if (enemy.name.Contains("White Palace Fly"))
            {
                Log("lol you don't get a hp bar");
                return false;
            }

            if (!isBoss)
            {
                HPBar hpbar = hm.gameObject.GetComponent<HPBar>();
                if (hpbar != null || hm.hp >= 5000 || isAlreadyDead) return isAlreadyDead;
                hm.gameObject.AddComponent<HPBar>();
                Log($@"Added hp bar to {enemy.name}");
            }
            else
            {
                BossHPBar bossHpBar = hm.gameObject.GetComponent<BossHPBar>();
                if (bossHpBar != null || hm.hp >= 5000 || isAlreadyDead) return isAlreadyDead;
                hm.gameObject.AddComponent<BossHPBar>();
                ActiveBosses.Add(enemy.name);
                Log($@"Added hp bar to boss {enemy.name}");
            }

            return false;
        }
        internal static void GetSkinList()
        {
            var dicts = Directory.GetDirectories(DATA_DIR);
            SkinList = new();
            for(int i=0;i<dicts.Length;i++)
            {
                string directoryname = new DirectoryInfo(dicts[i]).Name;
                SkinList.Add(new HPBarList(directoryname));
            }
            EnemyHPBar.instance.CurrentSkin = BetterMenu.GetSkinById(EnemyHPBar.instance.globalSettings.DefaultSkin);
            Modding.Logger.Log("Load Skinslist");
        }
        public static List<string> ActiveBosses;

        private static readonly FieldInfo DEATH_FI = typeof(EnemyDeathEffects).GetField("enemyDeathType", BindingFlags.NonPublic | BindingFlags.Instance);
        public static List<ISelectableSkin> SkinList;
        public ISelectableSkin CurrentSkin;
        public ISelectableSkin DefaultSkin;
    }
}
