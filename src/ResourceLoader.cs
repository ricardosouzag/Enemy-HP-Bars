using System.IO;

using UnityEngine;

namespace EnemyHPBar {
	internal class ResourceLoader : MonoBehaviour {
		public static byte[] GetBackgroundImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BG);

		public static byte[] GetForegroundImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_FG);

		public static byte[] GetMiddlegroundImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_MG);

		public static byte[] GetOutlineImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_OL);

		public static byte[] GetBossBackgroundImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BOSSBG);

		public static byte[] GetBossForegroundImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BOSSFG);

		public static byte[] GetBossOutlineImage() => File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BOSSOL);
	}
}