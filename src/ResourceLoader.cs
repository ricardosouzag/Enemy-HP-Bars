using EnemyHPBar.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Modding;
using System.IO;

namespace EnemyHPBar
{
    class ResourceLoader
    {
        public static byte[] GetBackgroundImage()
        {
            return Properties.Resources.bg;
        }

        public static byte[] GetForegroundImage()
        {
            return Properties.Resources.fg;
        }

        public static byte[] GetMiddlegroundImage()
        {
            return Properties.Resources.mg;
        }

        public static byte[] GetOutlineImage()
        {
            return Properties.Resources.outline;
        }

        public static byte[] GetBossBackgroundImage()
        {
            return Properties.Resources.bossbg;
        }

        public static byte[] GetBossForegroundImage()
        {
            return Properties.Resources.bossfg;
        }

        public static byte[] GetBossOutlineImage()
        {
            return Properties.Resources.bossoutline;
        }
    }
}