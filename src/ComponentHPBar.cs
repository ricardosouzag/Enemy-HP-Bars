using Modding;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
namespace EnemyHPBar
{
    public class HPBar : MonoBehaviour
    {
        public Sprite bg;
        public Sprite mg;
        public Sprite fg;
        public Sprite ol;

        private GameObject canvas;
        private GameObject bg_go;
        private GameObject mg_go;
        private GameObject fg_go;
        private GameObject ol_go;

        
        public CanvasGroup canvasGroup;
        public Image health_bar;
        public Image hpbg;

        public float currHP;
        public float maxHP;
        public int oldHP;

        
        public HealthManager hm;

        public Vector2 objectPos;
        public Vector2 screenScale;


        public void Awake()
        {
            Modding.Logger.Log($@"Creating canvas for {gameObject.name}");

            On.CameraController.FadeOut += CameraController_FadeOut;

            bg = CanvasUtil.CreateSprite(ResourceLoader.GetBackgroundImage(), 0, 0, 175, 19);
            mg = CanvasUtil.CreateSprite(ResourceLoader.GetMiddlegroundImage(), 0, 0, 117, 10);
            fg = CanvasUtil.CreateSprite(ResourceLoader.GetForegroundImage(), 0, 0, 117, 10);
            ol = CanvasUtil.CreateSprite(ResourceLoader.GetOutlineImage(), 0, 0, 175, 19);

            canvas = CanvasUtil.CreateCanvas(RenderMode.WorldSpace, new Vector2(16f, 9f));
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            canvas.GetComponent<Canvas>().sortingOrder = 1;

            screenScale = new Vector2(Screen.width/1280f * 0.025f, Screen.height/720f * 0.025f);

            bg_go = CanvasUtil.CreateImagePanel(canvas, bg,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(175, 19), screenScale), new Vector2(0, 32)));
            mg_go = CanvasUtil.CreateImagePanel(canvas, mg,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(117, 10), screenScale), new Vector2(0, 32)));
            fg_go = CanvasUtil.CreateImagePanel(canvas, fg,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(117, 10), screenScale), new Vector2(0, 32)));
            ol_go = CanvasUtil.CreateImagePanel(canvas, ol,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(175, 19), screenScale), new Vector2(0, 32)));

            hpbg = mg_go.GetComponent<Image>();
            hpbg.type = Image.Type.Filled;
            hpbg.fillMethod = Image.FillMethod.Horizontal;
            hpbg.preserveAspect = false;

            health_bar = fg_go.GetComponent<Image>();
            health_bar.type = Image.Type.Filled;
            health_bar.fillMethod = Image.FillMethod.Horizontal;
            health_bar.preserveAspect = false;

            bg_go.GetComponent<Image>().preserveAspect = false;
            ol_go.GetComponent<Image>().preserveAspect = false;

            DontDestroyOnLoad(canvas);

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
            if (currHP > hm.hp)
            {
                currHP -= 0.5f;
            }
            else
            {
                currHP = hm.hp;
            }
            Modding.Logger.LogDebug($@"Enemy currHP {hm.hp} and maxHP {maxHP}");
            health_bar.fillAmount = hm.hp / maxHP;

            hpbg.fillAmount = currHP / maxHP;

            if (health_bar.fillAmount < 1f)
            {
                canvasGroup.alpha = 1;
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
            oldHP = hm.hp;
        }

        void LateUpdate()
        {
            objectPos = gameObject.transform.position + Vector3.up * 1.5f;
            fg_go.transform.position = objectPos;
            mg_go.transform.position = objectPos;
            bg_go.transform.position = objectPos;
            ol_go.transform.position = objectPos;
        }
    }
}
