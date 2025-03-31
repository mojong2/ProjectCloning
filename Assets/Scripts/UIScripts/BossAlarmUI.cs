using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class BossAlarmUI : UIBase
    {
        [SerializeField] Animator animator;
        private void OnEnable()
        {
            GameManager.OnBossSpawn += BossSpawn;
        }
        private void OnDisable()
        {
            GameManager.OnBossSpawn -= BossSpawn;
        }
        void BossSpawn()
        {
            animator.SetTrigger("Replay");
        }
    }

}
