using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using static Project_J.RoundDatas;

namespace Project_J
{
    public class BossScripts : CharacterScripts
    {
        int reward;
        public int Reward => reward;
        [SerializeField] bool checkBoss;
        [SerializeField] MainCharacter mainCharacter;
        [SerializeField] TextMeshProUGUI hpText;
        [SerializeField] TextMeshProUGUI timerText;

        private void OnEnable()
        {
            this.tag = "Enemy";
            if (gameManager != null)
            {
                if (checkBoss)
                {
                    gameManager.OnTimerUpdated += ChangeTimerText;
                }
                else
                {
                    gameManager.OnHauntLimitTimerUpdated += ChangeHauntTimerText;
                }
            }
            SetWalkState();
        }
        private void OnDisable()
        {
            if (gameManager != null)
            {
                if (checkBoss)
                {
                    gameManager.OnTimerUpdated -= ChangeTimerText;
                }
                else 
                {
                    gameManager.OnHauntLimitTimerUpdated -= ChangeHauntTimerText;
                }
            } 
        }
        void ChangeHauntTimerText(string time)
        {
            timerText.text = time;
        }

        void ChangeTimerText(int time)
        {
            timerText.text = time.ToString();
            if (time <= 5) timerText.color = Color.red;
            else timerText.color = Color.white;
        }
        void AttackFunction()
        {
            mainCharacter.TakeDamage(attackDamage);
        }
        public void SetStat(RoundData roundData, GameManager gm, bool check)
        {
            reward = roundData.reward;
            checkBoss = check;
            gameManager = gm;
            moveSpeed = 0.3f;
            MaxHpValue = roundData.monsterHealth;
            maxHpRatio = 1.0f / MaxHpValue;
            HpValue = roundData.monsterHealth;
            attackDamage = roundData.monsterAttack;
            attackCoolTime = roundData.monsterAttackSpeed;
        }
        public override async void TakeDamage(int dmg)
        {
            base.TakeDamage(dmg);
            if (hpValue <= 0)
            {
                hpText.text = 0.ToString();
                SetDeadState();
                await UniTask.WaitForSeconds(0.5f);
                this.gameObject.SetActive(false);
            }
            else
            {
                hpText.text = hpValue.ToString();
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