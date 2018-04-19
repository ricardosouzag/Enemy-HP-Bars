using Modding;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EnemyHPBar
{
    public partial class ComponentHPBar : MonoBehaviour
    {
        public Sprite bg;
        public  Sprite fg;
        public  Sprite ol;

        private  GameObject canvas;
        private  GameObject bg_go;
        private  GameObject fg_go;
        private  GameObject ol_go;

        public RectTransform canvasRect;
        public  CanvasGroup canvasGroup;
        public  Image health_bar;
        
        public int maxHP;
        public bool dead;

        public PlayMakerFSM enemyFSM;
        public PlayMakerFSM hm;

        public Vector2 uiOffset;
        public Vector2 viewportPosition;
        public Vector2 objectPos;

        public BoxCollider2D collider;

        public void Awake()
        {
            Modding.Logger.Log($@"Creating canvas for {gameObject.name}");

            ModHooks.Instance.OnGetEventSenderHook += Instance_OnGetEventSenderHook;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;

            collider = gameObject.GetComponent<BoxCollider2D>();

            bg = CanvasUtil.CreateSprite(ResourceLoader.GetBackgroundImage(), 0, 0, 125, 33);
            fg = CanvasUtil.CreateSprite(ResourceLoader.GetForegroundImage(), 0, 0, 125, 33);

            canvas = CanvasUtil.CreateCanvas(RenderMode.ScreenSpaceOverlay, new Vector2(1280, 720));
            canvasGroup = canvas.GetComponent<CanvasGroup>();

            bg_go = CanvasUtil.CreateImagePanel(canvas, bg, new CanvasUtil.RectData(new Vector2(125, 33), new Vector2(0, 32), new Vector2(0.5f, 0), new Vector2(0.5f, 0)));
            fg_go = CanvasUtil.CreateImagePanel(canvas, fg, new CanvasUtil.RectData(new Vector2(125, 33), new Vector2(0, 32), new Vector2(0.5f, 0), new Vector2(0.5f, 0)));

            health_bar = fg_go.GetComponent<Image>();

            health_bar.type = Image.Type.Filled;
            health_bar.fillMethod = Image.FillMethod.Horizontal;
            health_bar.preserveAspect = false;

            bg_go.GetComponent<Image>().preserveAspect = false;

            MonoBehaviour.DontDestroyOnLoad(canvas);

            canvasRect = canvas.GetComponent<RectTransform>();
            viewportPosition = Camera.main.WorldToViewportPoint(gameObject.transform.position);
            
            objectPos = new Vector2(((viewportPosition.x * canvasRect.sizeDelta.x)),((viewportPosition.y * canvasRect.sizeDelta.y)));

            hm = FSMUtility.LocateFSM(gameObject, "health_manager") ?? FSMUtility.LocateFSM(gameObject, "health_manager_enemy");
        }

        private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
        {
            ModHooks.Instance.OnGetEventSenderHook -= Instance_OnGetEventSenderHook;
            canvasGroup.alpha = 0;
            Destroy(this);
        }

        public GameObject Instance_OnGetEventSenderHook(GameObject go, HutongGames.PlayMaker.Fsm fsm)
        {

            enemyFSM = fsm.FsmComponent;
            if (fsm.GameObject.name == gameObject.name)
            {
                try
                {
                    Modding.Logger.Log($@"{fsm.GameObjectName}'s HP before hit = {hm.FsmVariables.GetFsmInt("HP").Value}");

                    if (GameManager.instance.sceneName == "Room_Final_Boss_Core")
                    {
                        fsm.GameObject.name = "THK";
                    }

                    Modding.Logger.Log($@"Fill Amount - {(float)hm.FsmVariables.GetFsmInt("HP").Value / EnemyHPBar.healths[gameObject.name][0]}");
                    health_bar.fillAmount = (float)hm.FsmVariables.GetFsmInt("HP").Value / EnemyHPBar.healths[gameObject.name][0];

                    canvasGroup.alpha = 1;
                }
                catch
                {
                    Modding.Logger.Log($@"Error while running OnGetEventSenderHook for enemy {fsm.GameObject.name}");
                }        
            }
            return go;
        }

        void Update()
        {
            if (HeroController.instance.cState.dead)
                canvasGroup.alpha = 0;
            if (!EnemyHPBar.healths.ContainsKey(gameObject.name))
                canvasGroup.alpha = 0;

            health_bar.fillAmount = (float)hm.FsmVariables.GetFsmInt("HP").Value / EnemyHPBar.healths[gameObject.name][0];

            viewportPosition = Camera.main.WorldToViewportPoint(collider.transform.position);
            objectPos = new Vector2(viewportPosition.x * canvasRect.sizeDelta.x, (viewportPosition.y + 0.15f) * canvasRect.sizeDelta.y);
            fg_go.transform.position = objectPos;
            bg_go.transform.position = objectPos;
        }
    }
}
