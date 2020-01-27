using Modding;
using UnityEngine;
using UnityEngine.UI;
using GlobalEnums;

namespace EnemyHPBar
{
    public class BossHPBar : MonoBehaviour
    {
        public Sprite bg;
        public Sprite fg;
        public Sprite ol;
        private GameObject canvas;
        private GameObject bg_go;
        private GameObject fg_go;
        private GameObject ol_go;
        public CanvasGroup canvasGroup;
        public Image health_bar;
        public float currHP;
        public float maxHP;
        public int position;
        public HealthManager hm;
        public Vector2 objectPos;

        public void Awake()
        {
            Modding.Logger.LogDebug($@"Creating canvas for {gameObject.name}");
            Modding.Logger.LogDebug($"{gameObject.name} is boss");

            On.CameraController.FadeOut += CameraController_FadeOut;


            bg = CanvasUtil.CreateSprite(ResourceLoader.GetBossBackgroundImage(), 0, 0, 1, 1);
            fg = CanvasUtil.CreateSprite(ResourceLoader.GetBossForegroundImage(), 0, 0, 960, 1);
            ol = CanvasUtil.CreateSprite(ResourceLoader.GetBossOutlineImage(), 0, 0, 966, 27);

            canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1280f, 720f));
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            canvas.GetComponent<Canvas>().sortingOrder = 1;

            bg_go = CanvasUtil.CreateImagePanel(canvas, bg,
                new CanvasUtil.RectData(new Vector2(960f, 25f), new Vector2(0f, 32f), new Vector2(0.5f, 0f),
                    new Vector2(0.5f, 0f)));
            fg_go = CanvasUtil.CreateImagePanel(canvas, fg,
                new CanvasUtil.RectData(new Vector2(960f, 25f), new Vector2(0f, 32f), new Vector2(0.5f, 0f),
                    new Vector2(0.5f, 0f)));
            ol_go = CanvasUtil.CreateImagePanel(canvas, ol,
                new CanvasUtil.RectData(new Vector2(966f, 27f), new Vector2(0f, 32f), new Vector2(0.5f, 0f),
                    new Vector2(0.5f, 0f)));

            objectPos = fg_go.transform.position;

            health_bar = fg_go.GetComponent<Image>();
            health_bar.type = Image.Type.Filled;
            health_bar.fillMethod = Image.FillMethod.Horizontal;
            health_bar.preserveAspect = false;

            bg_go.GetComponent<Image>().preserveAspect = false;
            ol_go.GetComponent<Image>().preserveAspect = false;
            
            hm = gameObject.GetComponent<HealthManager>();

            canvasGroup.alpha = 0;

            maxHP = hm.hp;
            currHP = hm.hp;
        }

        private void CameraController_FadeOut(On.CameraController.orig_FadeOut orig, CameraController self, GlobalEnums.CameraFadeType type)
        {
            Destroy(this);
            orig(self, type);
        }

        void OnDestroy()
        {
            Modding.Logger.LogDebug($@"Destroying enemy {gameObject.name}");
            canvasGroup.alpha = 0;
            Destroy(this);
            Modding.Logger.LogDebug($@"Destroyed enemy {gameObject.name}");
            EnemyHPBar.ActiveBosses.Remove(gameObject.name);
        }

        void OnDisable()
        {
            Modding.Logger.LogDebug($@"Disabling enemy {gameObject.name}");
            canvasGroup.alpha = 0;
            Destroy(this);
            Modding.Logger.LogDebug($@"Disabled enemy {gameObject.name}");
        }

        void FixedUpdate()
        {
            position = EnemyHPBar.ActiveBosses.IndexOf(gameObject.name) + 1;
            if (currHP > hm.hp)
            {
                currHP -= 0.3f;
            }
            else
            {
                currHP = hm.hp;
            }

            Modding.Logger.LogDebug($@"Enemy currHP {hm.hp} and maxHP {maxHP}");
            health_bar.fillAmount = hm.hp / maxHP;
            if (health_bar.fillAmount < 1f && health_bar.fillAmount > 0f)
            {
                canvasGroup.alpha = GameManager.instance.gameState == GameState.PAUSED ? 0.8f : 1;
            }

            if (gameObject.name == "New Game Object" && currHP <= 0)
            {
                Modding.Logger.LogDebug($@"Placeholder killed");
                Destroy(gameObject);
            }

            if (currHP <= 0f)
            {
                canvasGroup.alpha = 0;
            }
        }

        void LateUpdate()
        {
            position = EnemyHPBar.ActiveBosses.IndexOf(gameObject.name);
            fg_go.transform.position = new Vector2(objectPos.x, objectPos.y + position * 30f);
            bg_go.transform.position = new Vector2(objectPos.x, objectPos.y + position * 30f);
            ol_go.transform.position = new Vector2(objectPos.x, objectPos.y + position * 30f);
        }
    }
}
