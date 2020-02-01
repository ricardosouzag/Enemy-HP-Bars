using System.Collections.Generic;
using System.Reflection;
using HutongGames.PlayMaker.Actions;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnemyHPBar
{
    public class EnemyHPBar : Mod
    {
        private const string version = "1.3.0";

        public static GameObject canvas;
        public static GameObject bossCanvas;
        
        public static Sprite bg;
        public static Sprite mg;
        public static Sprite fg;
        public static Sprite ol;
        public static Sprite bossbg;
        public static Sprite bossfg;
        public static Sprite bossol;
        

        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");

            ModHooks.Instance.OnEnableEnemyHook += Instance_OnEnableEnemyHook;
            ModHooks.Instance.OnRecieveDeathEventHook += Instance_OnRecieveDeathEventHook;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            
            

            canvas = CanvasUtil.CreateCanvas(RenderMode.WorldSpace, new Vector2(16f, 9f));
            bossCanvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1280f, 720f));
            canvas.GetComponent<Canvas>().sortingOrder = 1;
            bossCanvas.GetComponent<Canvas>().sortingOrder = 1;
            
            

            bg = CanvasUtil.CreateSprite(ResourceLoader.GetBackgroundImage(), 0, 0, 175, 19);
            mg = CanvasUtil.CreateSprite(ResourceLoader.GetMiddlegroundImage(), 0, 0, 117, 10);
            fg = CanvasUtil.CreateSprite(ResourceLoader.GetForegroundImage(), 0, 0, 117, 10);
            ol = CanvasUtil.CreateSprite(ResourceLoader.GetOutlineImage(), 0, 0, 175, 19);
            bossbg = CanvasUtil.CreateSprite(ResourceLoader.GetBossBackgroundImage(), 0, 0, 1, 1);
            bossfg = CanvasUtil.CreateSprite(ResourceLoader.GetBossForegroundImage(), 0, 0, 960, 1);
            bossol = CanvasUtil.CreateSprite(ResourceLoader.GetBossOutlineImage(), 0, 0, 966, 27);

            Object.DontDestroyOnLoad(canvas);
            Object.DontDestroyOnLoad(bossCanvas);

            Log("Initialized EnemyHPBars");
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            ActiveBosses = new List<string>();
        }

        private bool Instance_OnRecieveDeathEventHook(EnemyDeathEffects enemyDeathEffects, bool eventAlreadyRecieved, ref float? attackDirection, ref bool resetDeathEvent, ref bool spellBurn, ref bool isWatery)
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

            return eventAlreadyRecieved;
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
        public static List<string> ActiveBosses;

        private static readonly FieldInfo DEATH_FI = typeof(EnemyDeathEffects).GetField("enemyDeathType", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}
