using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnemyHPBar
{
    public class EnemyHPBar : Mod
    {

        private static string version = "1.1.2";

        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");

            ModHooks.Instance.OnEnableEnemyHook += Instance_OnEnableEnemyHook;
            ModHooks.Instance.OnRecieveDeathEventHook += Instance_OnRecieveDeathEventHook;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            if (bossList == null)
            {
                bossList = new[]
                {
                    "Head",
                    "False Knight New",
                    "Hornet Boss 1",
                    "Giant Fly",
                    "Mega Jellyfish",
                    "Mawlek Body",
                    "Mawlek Head",
                    "Mantis Lord",
                    "Mantis Lord S1",
                    "Mantis Lord S2",
                    "Mage Lord",
                    "Mage Lord Phase2",
                    "Infected Knight",
                    "Mega Zombie Beam Miner (1)",
                    "Zombie Beam Miner Rematch",
                    "Dung Defender",
                    "Mimic Spider",
                    "Hornet Boss 2",
                    "Fluke Mother",
                    "Mantis Traitor Lord",
                    "Grimm Boss",
                    "Black Knight",
                    "Jar Collector",
                    "Lost Kin",
                    "Nightmare Grimm Boss",
                    "False Knight Dream",
                    "White Defender",
                    "Dream Mage Lord",
                    "Dream Mage Lord Phase2",
                    "Grey Prince",
                    "Hive Knight",
                    "Radiance"
                };
                Log("Created boss list!");
            }

            Log("Initialized EnemyHPBars");
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            activeBosses = new List<string>();
        }

        private bool Instance_OnRecieveDeathEventHook(EnemyDeathEffects enemyDeathEffects, bool eventAlreadyRecieved, ref float? attackDirection, ref bool resetDeathEvent, ref bool spellBurn, ref bool isWatery)
        {
            Log($@"Enemy {enemyDeathEffects.gameObject.name} ded");
            if (enemyDeathEffects.gameObject.GetComponent<HPBar>().oldHP == 0)
            {
                GameObject placeHolder = new GameObject();
                placeHolder.transform.position = enemyDeathEffects.gameObject.transform.position;
                HealthManager phhm = placeHolder.AddComponent<HealthManager>();
                HPBar phhp = placeHolder.AddComponent<HPBar>();
                phhp.currHP = 18;
                phhp.maxHP = 18;
                phhm.hp = 0;
            }
            else
            {
                GameObject placeHolder = new GameObject();
                placeHolder.transform.position = enemyDeathEffects.gameObject.transform.position;
                HealthManager phhm = placeHolder.AddComponent<HealthManager>();
                HPBar phhp = placeHolder.AddComponent<HPBar>();
                phhp.currHP = enemyDeathEffects.gameObject.GetComponent<HPBar>().oldHP;
                phhp.maxHP = enemyDeathEffects.gameObject.GetComponent<HPBar>().maxHP;
                phhm.hp = 0;
            }

            return eventAlreadyRecieved;
        }

        private bool Instance_OnEnableEnemyHook(GameObject enemy, bool isAlreadyDead)
        {
            HealthManager hm = enemy.GetComponent<HealthManager>();
            if (!bossList.Contains(enemy.name))
            {
                HPBar hpbar = hm.gameObject.GetComponent<HPBar>();
                if (hpbar != null || hm.hp >= 5000 || hm == null || isAlreadyDead) return isAlreadyDead;
                hm.gameObject.AddComponent<HPBar>();
                LogDebug($@"Added hp bar to {enemy.name}");
            }
            else
            {
                BossHPBar bossHpBar = hm.gameObject.GetComponent<BossHPBar>();
                if (bossHpBar != null || hm.hp >= 5000 || hm == null || isAlreadyDead) return isAlreadyDead;
                hm.gameObject.AddComponent<BossHPBar>();
                activeBosses.Add(enemy.name);
                LogDebug($@"Added hp bar to boss {enemy.name}");
            }

            return isAlreadyDead;
        }
        public static string[] bossList;
        public static List<string> activeBosses;
    }
}
