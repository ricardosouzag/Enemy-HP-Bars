using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Modding;
using GlobalEnums;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace EnemyHPBar
{
    public class EnemyHPBar : Mod
    {

        private static string version = "0.1.0";

        public static Dictionary<GameObject, int> healths;
        public static Dictionary<GameObject, ComponentHPBar> hpbars;
        public static Dictionary<GameObject, string> enemies;

        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");
            
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += ClearingLists;
            ModHooks.Instance.HeroUpdateHook += Instance_HeroUpdateHook;


            Log("Initialized EnemyHPBars");
        }


        private void Instance_HeroUpdateHook()
        {
            foreach(HealthManager hm in GameObject.FindObjectsOfType<HealthManager>())
            {
                if (!healths.Keys.Any(f => f == hm.gameObject) && hm.hp < 5000 && hm != null)
                {
                    healths.Add(hm.gameObject, hm.hp);
                    enemies.Add(hm.gameObject, hm.gameObject.name);
                    ComponentHPBar hpbar = hm.gameObject.AddComponent<ComponentHPBar>();
                    hpbars.Add(hm.gameObject, hpbar);
                    Log($@"Enemies: {String.Join(",\n", enemies.Values.ToArray())}");
                }
            }
        }

        public void ClearingLists(Scene s, LoadSceneMode lsm)
        {
            healths = new Dictionary<GameObject, int> ();
            hpbars = new Dictionary<GameObject, ComponentHPBar>();
            enemies = new Dictionary<GameObject, string>();
        }        
    }
}
