using Modding;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable InconsistentNaming
namespace EnemyHPBar
{
    public class HPBar : MonoBehaviour
    {
        private GameObject bg_go;
        private GameObject mg_go;
        private GameObject fg_go;
        private GameObject ol_go;
        private CanvasRenderer bg_cr;
        private CanvasRenderer fg_cr;
        private CanvasRenderer mg_cr;
        private CanvasRenderer ol_cr;

        
        public Image health_bar;
        public Image hpbg;

        public float currHP;
        public float maxHP;
        public int oldHP;

        
        public HealthManager hm;

        public Vector2 objectPos;
        public Vector2 screenScale;
        
        private float bgScale = EnemyHPBar.instance.globalSettings.bgScale;
        private float fgScale = EnemyHPBar.instance.globalSettings.fgScale;
        private float olScale = EnemyHPBar.instance.globalSettings.olScale;
        private float mgScale = EnemyHPBar.instance.globalSettings.mgScale;


        public void Awake()
        {
            Modding.Logger.Log($@"Creating hpbar for {gameObject.name}");

//            On.CameraController.FadeOut += CameraController_FadeOut;
            
            screenScale = new Vector2(Screen.width/1280f * 0.025f, Screen.height/720f * 0.025f);

            bg_go = CanvasUtil.CreateImagePanel(EnemyHPBar.canvas, EnemyHPBar.bg,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.bg.texture.width, EnemyHPBar.bg.texture
                .height), screenScale * bgScale), new Vector2(0, 
                32)));
            mg_go = CanvasUtil.CreateImagePanel(EnemyHPBar.canvas, EnemyHPBar.mg,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.mg.texture.width, EnemyHPBar.mg.texture
                    .height), screenScale * mgScale), new Vector2(0, 32)));
            fg_go = CanvasUtil.CreateImagePanel(EnemyHPBar.canvas, EnemyHPBar.fg,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.fg.texture.width, EnemyHPBar.fg.texture
                    .height), screenScale * fgScale), new Vector2(0, 32)));
            ol_go = CanvasUtil.CreateImagePanel(EnemyHPBar.canvas, EnemyHPBar.ol,
                new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.ol.texture.width, EnemyHPBar.ol.texture
                    .height), screenScale * olScale), new Vector2(0, 32)));

            bg_cr = bg_go.GetComponent<CanvasRenderer>();
            fg_cr = fg_go.GetComponent<CanvasRenderer>();
            mg_cr = mg_go.GetComponent<CanvasRenderer>();
            ol_cr = ol_go.GetComponent<CanvasRenderer>();

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

            hm = gameObject.GetComponent<HealthManager>();
            
            SetHPBarAlpha(0);
            maxHP = hm.hp;
            currHP = hm.hp;
        }

        private void SetHPBarAlpha(float alpha)
        {
            bg_cr.SetAlpha(alpha);
            fg_cr.SetAlpha(alpha);
            mg_cr.SetAlpha(alpha);
            ol_cr.SetAlpha(alpha);
        }

        private void DestroyHPBar()
        {
            Destroy(fg_go);
            Destroy(bg_go);
            Destroy(mg_go);
            Destroy(ol_go);
            Destroy(hpbg);
            Destroy(health_bar);
        }

        private void MoveHPBar(Vector2 position)
        {
            fg_go.transform.position = position;
            mg_go.transform.position = position;
            bg_go.transform.position = position;
            ol_go.transform.position = position;
        }
        
        void OnDestroy()
        {
            Modding.Logger.LogDebug($@"Destroying enemy {gameObject.name}");
            SetHPBarAlpha(0);
            DestroyHPBar();
            Modding.Logger.LogDebug($@"Destroyed enemy {gameObject.name}");
        }

        void OnDisable()
        {       
            Modding.Logger.LogDebug($@"Disabling enemy {gameObject.name}");
            SetHPBarAlpha(0);
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
                SetHPBarAlpha(1);
            }
            if (gameObject.name == "New Game Object" && currHP <= 0)
            {
                Modding.Logger.LogDebug($@"Placeholder killed");
                Destroy(gameObject);
            }

            if (currHP <= 0f)
            {
                SetHPBarAlpha(0);
            }
            oldHP = hm.hp;
        }

        void LateUpdate()
        {
            objectPos = gameObject.transform.position + Vector3.up * 1.5f;
            MoveHPBar(objectPos);
        }
    }
}
