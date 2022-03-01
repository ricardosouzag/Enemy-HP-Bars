namespace EnemyHPBar;

internal sealed class ResourceLoader : MonoBehaviour {
	private static byte[] GetImage(string name) => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId(), name));

	public static byte[] GetBackgroundImage() => GetImage(EnemyHPBar.HPBAR_BG);

	public static byte[] GetForegroundImage() => GetImage(EnemyHPBar.HPBAR_FG);

	public static byte[] GetMiddlegroundImage() => GetImage(EnemyHPBar.HPBAR_MG);

	public static byte[] GetOutlineImage() => GetImage(EnemyHPBar.HPBAR_OL);

	public static byte[] GetBossBackgroundImage() => GetImage(EnemyHPBar.HPBAR_BOSSBG);

	public static byte[] GetBossForegroundImage() => GetImage(EnemyHPBar.HPBAR_BOSSFG);

	public static byte[] GetBossOutlineImage() => GetImage(EnemyHPBar.HPBAR_BOSSOL);
}
