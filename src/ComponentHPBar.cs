using Modding;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyHPBar
{
    public partial class HPBar : MonoBehaviour
    {
        public Sprite bg;
        public Sprite mg;
        public Sprite fg;
        public Sprite ol;

        private  GameObject canvas;
        private  GameObject bg_go;
        private  GameObject mg_go;
        private  GameObject fg_go;
        private  GameObject ol_go;

        public RectTransform canvasRect;
        public CanvasGroup canvasGroup;
        public Image health_bar;
        public Image hpbg;
        
        public float currHP;
        public float maxHP;
        public int oldHP;
        public bool dead;

        public PlayMakerFSM enemyFSM;
        public HealthManager hm;

        public Vector2 uiOffset;
        public Vector2 viewportPosition;
        public Vector2 objectPos;

        public BoxCollider2D collider;

        public void Awake()
        {
            Modding.Logger.Log($@"Creating canvas for {gameObject.name}");

            bg = CanvasUtil.CreateSprite(ResourceLoader.GetBackgroundImage(), 0, 0, 175, 19);
            mg = CanvasUtil.CreateSprite(ResourceLoader.GetMiddlegroundImage(), 0, 0, 117, 10);
            fg = CanvasUtil.CreateSprite(ResourceLoader.GetForegroundImage(), 0, 0, 117, 10);
            ol = CanvasUtil.CreateSprite(ResourceLoader.GetOutlineImage(), 0, 0, 175, 19);

            canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(Screen.width, Screen.height));
            canvasGroup = canvas.GetComponent<CanvasGroup>();

            bg_go = CanvasUtil.CreateImagePanel(canvas, bg, new CanvasUtil.RectData(new Vector2(175, 19), new Vector2(0, 32)));
            mg_go = CanvasUtil.CreateImagePanel(canvas, mg, new CanvasUtil.RectData(new Vector2(117, 10), new Vector2(0, 32)));
            fg_go = CanvasUtil.CreateImagePanel(canvas, fg, new CanvasUtil.RectData(new Vector2(117, 10), new Vector2(0, 32)));
            ol_go = CanvasUtil.CreateImagePanel(canvas, ol, new CanvasUtil.RectData(new Vector2(175, 19), new Vector2(0, 32)));

            health_bar = fg_go.GetComponent<Image>();

            health_bar.type = Image.Type.Filled;
            health_bar.fillMethod = Image.FillMethod.Horizontal;
            health_bar.preserveAspect = false;


            hpbg = mg_go.GetComponent<Image>();

            hpbg.type = Image.Type.Filled;
            hpbg.fillMethod = Image.FillMethod.Horizontal;
            hpbg.preserveAspect = false;

            bg_go.GetComponent<Image>().preserveAspect = false;
            ol_go.GetComponent<Image>().preserveAspect = false;

            MonoBehaviour.DontDestroyOnLoad(canvas);

            canvasRect = canvas.GetComponent<RectTransform>();
            viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            
            objectPos = new Vector2(((viewportPosition.x * canvasRect.sizeDelta.x)),((viewportPosition.y * canvasRect.sizeDelta.y)));

            hm = gameObject.GetComponent<HealthManager>();

            canvasGroup.alpha = 0;
            maxHP = hm.hp;
            currHP = hm.hp;
        }


        void OnDestroy()
        {
            Modding.Logger.Log($@"Destroying enemy {gameObject.name}");
            canvasGroup.alpha = 0;
            Destroy(this);
            Modding.Logger.Log($@"Destroyed enemy {gameObject.name}");
        }

        void OnDisable()
        {       
            Modding.Logger.Log($@"Disabling enemy {gameObject.name}");
            canvasGroup.alpha = 0;
            Destroy(this);
            Modding.Logger.Log($@"Disabled enemy {gameObject.name}");
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
            health_bar.fillAmount = hm.hp / maxHP;
            hpbg.fillAmount = currHP / maxHP;
            if (health_bar.fillAmount != 1)
            {
                canvasGroup.alpha = 1;
            }
            if (gameObject.name == "New Game Object" && currHP <= 0)
            {
                Modding.Logger.Log($@"Placeholder killed");
                Destroy(gameObject);
            }
            oldHP = hm.hp;
        }

        void LateUpdate()
        {
            viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            objectPos = new Vector2(viewportPosition.x * canvasRect.sizeDelta.x, (viewportPosition.y + 0.15f) * canvasRect.sizeDelta.y);
            fg_go.transform.position = objectPos;
            mg_go.transform.position = objectPos;
            bg_go.transform.position = objectPos;
            ol_go.transform.position = objectPos;
        }
    }
}
