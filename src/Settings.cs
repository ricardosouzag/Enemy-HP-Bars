namespace EnemyHPBar; 

public sealed class Settings {
	public float fgScale = 1.0f;
	public float bgScale = 1.0f;
	public float mgScale = 1.0f;
	public float olScale = 1.0f;

	public float bossfgScale = 1.0f;
	public float bossbgScale = 1.0f;
	public float bossolScale = 1.0f;

	public int NameLength = 10;

	public string DefaultSkin { get; set; } = "Default";
	public string CurrentSkin { get; set; } = "Default";
}
