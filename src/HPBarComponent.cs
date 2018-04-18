using UnityEngine;
using Modding;
using System.IO;
using System;

namespace EnemyHPBar
{
    class HPBarComponent : MonoBehaviour
    {
        private static GameManager gm;
        private static UIManager uim;

        public void Start()
        {
            gm = GameManager.instance;
            uim = UIManager.instance;
        }

        public void Update()
        {
            if (EnemyHPBar.enemyFSM != null)
            {
                Modding.Logger.Log(EnemyHPBar.enemyFSM.FsmVariables.GetFsmInt("HP").Value);
                
            }
        }
    }
}
