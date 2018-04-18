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

        private static string version = "0.0.1";

        public static Dictionary<string, int[]> enemies;
        public static Dictionary<string, ComponentHPBar> hpbars;
        public static PlayMakerFSM enemyFSM;

        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");

            ModHooks.Instance.OnGetEventSenderHook += Instance_OnGetEventSenderHook;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += checkComponent;
            ModHooks.Instance.HeroUpdateHook += Instance_HeroUpdateHook;
            //ModHooks.Instance.SlashHitHook += Instance_SlashHitHook;


            Log("Initialized EnemyHPBars");
        }

        private void Instance_HeroUpdateHook()
        {
            foreach (KeyValuePair<string, ComponentHPBar> hpbar in hpbars)
            {
                if (GameObject.Find(hpbar.Key) == null || !GameObject.Find(hpbar.Key).activeSelf)
                {
                    EventInfo ei = ModHooks.Instance.GetType().GetEvent("OnGetEventSenderHook", BindingFlags.Instance | BindingFlags.Public);
                    if (ei != null)
                    {
                        Delegate handler = Delegate.CreateDelegate(ei.EventHandlerType, hpbar.Value, "Instance_OnGetEventSenderHook");
                        ei.RemoveEventHandler(ModHooks.Instance, handler);
                    }
                    UnityEngine.Object.Destroy(hpbar.Value);
                }
            }
        }

        public void checkComponent(Scene s, LoadSceneMode lsm)
        {
            enemies = new Dictionary<string, int[]> ();
            hpbars = new Dictionary<string, ComponentHPBar>();
            //foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
            //{
            //    PlayMakerFSM hme = FSMUtility.LocateFSM(go, "health_manager_enemy");
            //    PlayMakerFSM hm = FSMUtility.LocateFSM(go, "health_manager");
            //    if ( !Equals(hm, null) || !Equals(hme, null) )
            //    {
            //        enemies.Add(go.name, -1);
            //        go.AddComponent<ComponentHPBar>();
            //    }
            //}
            //Log($@"Enemies: {String.Join(",\n", enemies.Keys.ToArray())}");
        }

        GameObject Instance_OnGetEventSenderHook(GameObject go, HutongGames.PlayMaker.Fsm fsm)
        {

            PlayMakerFSM hm = FSMUtility.LocateFSM(fsm.GameObject, "health_manager") ?? FSMUtility.LocateFSM(fsm.GameObject, "health_manager_enemy");
            if (!Equals(hm, null) && Equals(fsm.GameObject.GetComponent<ComponentHPBar>(), null))
            {
                ComponentHPBar hp = fsm.GameObject.AddComponent<ComponentHPBar>();
                enemies.Add(fsm.GameObjectName, new int[2] { hm.FsmVariables.GetFsmInt("HP").Value, -1 });
                hpbars.Add(fsm.GameObjectName, hp);
                hp.Instance_OnGetEventSenderHook(go, fsm);
                Log($@"Enemies: {String.Join(",\n", enemies.Keys.ToArray())}");
            }
            return go;
        }
    }
}
