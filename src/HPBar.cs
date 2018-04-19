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

        public static Dictionary<string, int[]> healths;
        public static Dictionary<string, ComponentHPBar> hpbars;
        public static Dictionary<string, GameObject> enemies;
        public static List<string> dedList;
        public static PlayMakerFSM enemyFSM;

        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");

            ModHooks.Instance.OnGetEventSenderHook += Instance_OnGetEventSenderHook;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += ClearingLists;
            ModHooks.Instance.HeroUpdateHook += Instance_HeroUpdateHook;
            //ModHooks.Instance.SlashHitHook += Instance_SlashHitHook;


            Log("Initialized EnemyHPBars");
        }

        public GameObject[] FindGameObjectsByName(string name)
        {
            return Array.FindAll((UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[]), p => p.name == name);
        }

        private void Instance_HeroUpdateHook()
        {

            foreach (KeyValuePair<string, GameObject> ob in enemies)
            {
                GameObject go = ob.Value;
                if (go == null || !go.activeSelf || !go.activeInHierarchy)
                {
                    Log($@"Cleaning up after {ob.Key}");
                    EventInfo ei = ModHooks.Instance.GetType().GetEvent("OnGetEventSenderHook", BindingFlags.Instance | BindingFlags.Public);
                    if (ei != null)
                    {
                        Delegate handler = Delegate.CreateDelegate(ei.EventHandlerType, hpbars[ob.Key], "Instance_OnGetEventSenderHook");
                        ei.RemoveEventHandler(ModHooks.Instance, handler);
                    }
                    try
                    {
                        hpbars[ob.Key].canvasGroup.alpha = 0;
                    }
                    catch
                    {
                        Log("Error while cleaning canvas");
                    }
                    UnityEngine.Object.Destroy(hpbars[ob.Key]);
                    dedList.Add(ob.Key);
                }
            }
            foreach (string enemy in dedList)
            {
                healths.Remove(enemy);
                hpbars.Remove(enemy);
                enemies.Remove(enemy);

                Log($@"Finished cleaning up after {enemy}");
            }
            if (dedList != new List<string>())
            dedList = new List<string>();
        }

        public void ClearingLists(Scene s, LoadSceneMode lsm)
        {
            healths = new Dictionary<string, int[]> ();
            hpbars = new Dictionary<string, ComponentHPBar>();
            enemies = new Dictionary<string, GameObject>();
            dedList = new List<string>();
        }

        GameObject Instance_OnGetEventSenderHook(GameObject go, HutongGames.PlayMaker.Fsm fsm)
        {
            PlayMakerFSM hm = FSMUtility.LocateFSM(fsm.GameObject, "health_manager") ?? FSMUtility.LocateFSM(fsm.GameObject, "health_manager_enemy");

            if (!Equals(hm, null) && Equals(fsm.GameObject.GetComponent<ComponentHPBar>(), null))
            {
                ComponentHPBar hp = fsm.GameObject.AddComponent<ComponentHPBar>();

                healths.Add(fsm.GameObjectName, new int[2] { hm.FsmVariables.GetFsmInt("HP").Value, -1});
                enemies.Add(fsm.GameObjectName, fsm.GameObject);
                hpbars.Add(fsm.GameObject.name, hp);
                
                hp.Instance_OnGetEventSenderHook(go, fsm);

                Log($@"Enemies: {String.Join(",\n", healths.Keys.ToArray())}");
            }

            return go;
        }
    }
}
