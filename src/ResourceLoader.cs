using System.IO;
using UnityEngine;

namespace EnemyHPBar
{
    class ResourceLoader : MonoBehaviour 
    {
        public static byte[] GetBackgroundImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR,EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BG);
        }

        public static byte[] GetForegroundImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_FG);
        }

        public static byte[] GetMiddlegroundImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_MG);
        }

        public static byte[] GetOutlineImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_OL);
        }

        public static byte[] GetBossBackgroundImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BOSSBG);
        }

        public static byte[] GetBossForegroundImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BOSSFG);
        }

        public static byte[] GetBossOutlineImage()
        {
            return File.ReadAllBytes(Path.Combine(EnemyHPBar.DATA_DIR, EnemyHPBar.instance.CurrentSkin.GetId()) + "/" + EnemyHPBar.HPBAR_BOSSOL);
        }
    }
}