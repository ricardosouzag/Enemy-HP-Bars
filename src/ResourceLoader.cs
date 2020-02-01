using System.IO;
using EnemyHPBar.Properties;
using UnityEngine;

namespace EnemyHPBar
{
    class ResourceLoader : MonoBehaviour 
    {
        public static byte[] GetBackgroundImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_BG);
        }

        public static byte[] GetForegroundImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_FG);
        }

        public static byte[] GetMiddlegroundImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_MG);
        }

        public static byte[] GetOutlineImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_OL);
        }

        public static byte[] GetBossBackgroundImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_BOSSBG);
        }

        public static byte[] GetBossForegroundImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_BOSSFG);
        }

        public static byte[] GetBossOutlineImage()
        {
            return File.ReadAllBytes(EnemyHPBar.DATA_DIR + "/" + EnemyHPBar.HPBAR_BOSSOL);
        }
    }
}