using MonoMod.ModInterop;

namespace EnemyHPBar;

[ModExportName(nameof(EnemyHPBar))]
public static class EnemyHPBarExport {
	public static void DisableHPBar(GameObject go) {
		UObject.Destroy(go.GetComponent<HPBar>());
		UObject.Destroy(go.GetComponent<BossHPBar>());
		go.AddComponent<DisableHPBar>();
	}

	public static void EnableHPBar(GameObject go) {
		UObject.Destroy(go.GetComponent<DisableHPBar>());
		HealthManager hm = go.GetComponent<HealthManager>();
		hm.enabled = false;
		hm.enabled = true;
	}

	public static void MarkAsBoss(GameObject go) {
		if (go.GetComponent<BossMarker>() is BossMarker marker) {
			marker.isBoss = true;
		} else {
			go.AddComponent<BossMarker>();
		}

		DisableHPBar(go);
		EnableHPBar(go);
	}

	public static void MarkAsNonBoss(GameObject go) {
		if (go.GetComponent<BossMarker>() is BossMarker marker) {
			marker.isBoss = false;
		} else {
			go.AddComponent<BossMarker>().isBoss = false;
		}

		DisableHPBar(go);
		EnableHPBar(go);
	}
}
