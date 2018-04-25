using System;
using System.Collections.Generic;
using System.Linq;
using Modding;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnemyHPBar
{
    public class EnemyHPBar : Mod
    {

        private static string version = "0.3.0";

        public override string GetVersion()
        {
            return version;
        }

        public override void Initialize()
        {
            Log("Initializing EnemyHPBars");

            ModHooks.Instance.OnEnableEnemyHook += Instance_OnEnableEnemyHook;
            ModHooks.Instance.OnRecieveDeathEventHook += Instance_OnRecieveDeathEventHook;


            Log("Initialized EnemyHPBars");
        }

        private bool Instance_OnRecieveDeathEventHook(EnemyDeathEffects enemyDeathEffects, bool eventAlreadyRecieved, ref float? attackDirection, ref bool resetDeathEvent, ref bool spellBurn, ref bool isWatery)
        {
            LogDebug($@"Enemy {enemyDeathEffects.gameObject.name} ded");
            if (enemyDeathEffects.gameObject.GetComponent<HPBar>().oldHP == 0)
            {
                GameObject placeHolder = new GameObject();
                placeHolder.transform.position = enemyDeathEffects.gameObject.transform.position;
                HealthManager phhm = placeHolder.AddComponent<HealthManager>();
                HPBar phhp = placeHolder.AddComponent<HPBar>();
                phhp.currHP = 30;
                phhp.maxHP = 30;
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
            HPBar hpbar = hm.gameObject.GetComponent<HPBar>();
            if (hpbar == null && hm.hp < 5000 && hm != null && !isAlreadyDead)
            {
                HPBar bar = hm.gameObject.AddComponent<HPBar>();
                LogDebug($@"Added hp bar to {enemy.name}");
            }
            return isAlreadyDead;
        }
    }
}
