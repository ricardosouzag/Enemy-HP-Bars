using EnemyHPBar.Properties;

namespace EnemyHPBar
{
    class ResourceLoader
    {
        public static byte[] GetBackgroundImage()
        {
            return Resources.bg;
        }

        public static byte[] GetForegroundImage()
        {
            return Resources.fg;
        }

        public static byte[] GetMiddlegroundImage()
        {
            return Resources.mg;
        }

        public static byte[] GetOutlineImage()
        {
            return Resources.outline;
        }

        public static byte[] GetBossBackgroundImage()
        {
            return Resources.bossbg;
        }

        public static byte[] GetBossForegroundImage()
        {
            return Resources.bossfg;
        }

        public static byte[] GetBossOutlineImage()
        {
            return Resources.bossoutline;
        }
    }
}