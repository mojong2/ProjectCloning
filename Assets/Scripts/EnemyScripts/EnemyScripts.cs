using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static Project_J.RoundDatas;

namespace Project_J
{
    public class EnemyScripts : CharacterScripts
    {
        [SerializeField] MainCharacter mainCharacter;
        private void OnEnable()
        {
            this.tag = "Enemy";
            SetWalkState();
        }
        public void SetStat(RoundData roundData, GameManager gm)
        {
            gameManager = gm;
            moveSpeed = 0.3f;
            MaxHpValue = roundData.monsterHealth;
            maxHpRatio = 1.0f / MaxHpValue;
            HpValue = roundData.monsterHealth;
            attackDamage = roundData.monsterAttack;
            attackCoolTime = roundData.monsterAttackSpeed;
        }
        void AttackFunction()
        {
            mainCharacter.TakeDamage(attackDamage);
        }
        public override async void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            if(hpValue <= 0)
            {
                SetDeadState();
                await UniTask.WaitForSeconds(0.5f);
                this.gameObject.SetActive(false);
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.TryGetComponent(out MainCharacter character))
                {
                    mainCharacter = character; 
                    SetAttackState();
                }
            }
        }
    }
}