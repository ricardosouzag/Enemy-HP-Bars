namespace EnemyHPBar;

public class BossHPBar : MonoBehaviour {
	private GameObject bg_go;
	private GameObject fg_go;
	private GameObject ol_go;
	private CanvasRenderer bg_cr;
	private CanvasRenderer fg_cr;
	private CanvasRenderer ol_cr;

	private readonly float bossbgScale = EnemyHPBar.instance.globalSettings.bossbgScale;
	private readonly float bossfgScale = EnemyHPBar.instance.globalSettings.bossfgScale;
	private readonly float bossolScale = EnemyHPBar.instance.globalSettings.bossolScale;

	public Image health_bar;

	public float currHP;
	public float maxHP;

	public int position;
	public Vector2 screenScale;
	public HealthManager hm;
	public Vector2 objectPos;

	public void Awake() {
		Logger.LogDebug($@"Creating Boss HP Bar for {name}");

		screenScale = new Vector2(Screen.width / 1280f, Screen.height / 720f);

		bg_go = CanvasUtil.CreateImagePanel(EnemyHPBar.bossCanvas, EnemyHPBar.bossbg,
			new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.bossbg.texture.width, EnemyHPBar.bossbg
			.texture.height), screenScale * bossbgScale), new Vector2(0f, 32f),
			new Vector2(0.5f, 0f),
				new Vector2(0.5f, 0f)));
		fg_go = CanvasUtil.CreateImagePanel(EnemyHPBar.bossCanvas, EnemyHPBar.bossfg,
			new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.bossfg.texture.width, EnemyHPBar.bossfg
					.texture.height), screenScale * bossfgScale), new Vector2(0f, 32f), new Vector2(0.5f, 0f),
				new Vector2(0.5f, 0f)));
		ol_go = CanvasUtil.CreateImagePanel(EnemyHPBar.bossCanvas, EnemyHPBar.bossol,
			new CanvasUtil.RectData(Vector2.Scale(new Vector2(EnemyHPBar.bossol.texture.width, EnemyHPBar.bossol
					.texture.height), screenScale * bossolScale), new Vector2(0f, 32f), new Vector2(0.5f, 0f),
				new Vector2(0.5f, 0f)));

		bg_cr = bg_go.GetComponent<CanvasRenderer>();
		fg_cr = fg_go.GetComponent<CanvasRenderer>();
		ol_cr = ol_go.GetComponent<CanvasRenderer>();

		objectPos = fg_go.transform.position;

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

	public void OnEnable() {
		EnemyHPBar.ActiveBosses.Add(name);
	}

	private void SetHPBarAlpha(float alpha) {
		bg_cr.SetAlpha(alpha);
		fg_cr.SetAlpha(alpha);
		ol_cr.SetAlpha(alpha);
	}

	private void DestroyHPBar() {
		Destroy(fg_go);
		Destroy(bg_go);
		Destroy(ol_go);
		Destroy(health_bar);
	}

	private void MoveHPBar(Vector2 position) {
		fg_go.transform.position = position;
		bg_go.transform.position = position;
		ol_go.transform.position = position;
	}

	private void OnDestroy() {
		SetHPBarAlpha(0);
		DestroyHPBar();
		Logger.LogDebug($@"Destroyed enemy {gameObject.name}");
	}

	private void OnDisable() {
		EnemyHPBar.ActiveBosses.Remove(name);
		SetHPBarAlpha(0);
		Logger.LogDebug($@"Disabled enemy {gameObject.name}");
	}

	private void FixedUpdate() {
		position = EnemyHPBar.ActiveBosses.IndexOf(gameObject.name) + 1;
		if (currHP > hm.hp) {
			currHP -= 0.3f;
		} else {
			currHP = hm.hp;
		}

		Logger.LogFine($@"Enemy {name}: currHP {hm.hp}, maxHP {maxHP}");
		health_bar.fillAmount = hm.hp / maxHP;
		if (health_bar.fillAmount < 1f && health_bar.fillAmount > 0f) {
			float alpha = GameManager.instance.gameState == GameState.PAUSED ? 0.8f : 1;
			SetHPBarAlpha(alpha);
		}

		if (gameObject.name == "New Game Object" && currHP <= 0) {
			Destroy(gameObject);
			Logger.LogDebug($@"Placeholder killed");
		}

		if (currHP <= 0f) {
			SetHPBarAlpha(0);
		}
	}

	private void LateUpdate() {
		position = EnemyHPBar.ActiveBosses.IndexOf(gameObject.name);
		MoveHPBar(new Vector2(objectPos.x, objectPos.y + (position * 30f)));
	}
}
