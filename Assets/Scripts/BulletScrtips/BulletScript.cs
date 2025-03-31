using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Project_J.BulletDatas;

namespace Project_J
{
    public class BulletScript : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] Rigidbody2D rigidbody2D;
        [SerializeField] CircleCollider2D circleCollider2D;

        [SerializeField] MainCharacter mainCharacter;

        [SerializeField] int bulletDamage;
        [SerializeField] int hitCounter;
        float moveSpeed;
        [SerializeField] float maxDistance;
        [SerializeField] BulletType bulletType;

        float endTime;
        bool pungAnima;
        [SerializeField] bool splashDeadAttack;
        bool bulletIsDead;

        private void OnEnable()
        {
            if(moveSpeed > 0)
            {
                rigidbody2D.velocity = Vector2.down * moveSpeed;
                BulletMoveLimit();
            }
            else 
            {
                BulletTimerLimit();
            }
        }
        async void BulletTimerLimit()
        {
            float timer = 0.0f;
            while (this != null && this.gameObject.activeSelf)
            {
                if(timer > maxDistance)
                {
                    BulletActiveFalse();
                    break;
                }
                else timer += Time.deltaTime;
                await UniTask.Yield();
            }
        }
        async void BulletMoveLimit()
        {
            float starty = this.transform.position.y;
            float dis;
            while (this != null && this.gameObject.activeSelf)
            {
                dis = starty - this.transform.position.y;
                if(dis >= maxDistance)
                {
                    BulletActiveFalse();
                    break;
                }
                await UniTask.Yield();
            }

        }
        public void SetMainCharacter(MainCharacter chara)
        {
            mainCharacter = chara;
        }
        public void SetStat(BulletData bulletData, int force, MainCharacter chara)
        {
            bulletIsDead = false;
            if (mainCharacter == null)
            {
                mainCharacter = chara;
            }
            pungAnima = bulletData.pungAnimation;
            bulletType = bulletData.bulletType;
            circleCollider2D.radius = bulletData.circleSize;
            animator.runtimeAnimatorController = bulletData.animator;
            splashDeadAttack = bulletData.splashDeadAttack;
            bulletDamage = bulletData.damage * force;
            hitCounter = bulletData.hitCount;
            endTime = bulletData.endTime;
            moveSpeed = bulletData.bulletSpeed;
            maxDistance = bulletData.maxDistance;
        }
        async void BulletActiveFalse()
        {
            rigidbody2D.velocity = Vector2.zero;
            bulletIsDead = true;
            if (splashDeadAttack)
            {
                hitCounter = 99;
            }
            if (pungAnima)
            {
                animator.SetTrigger("End");
            }
            await UniTask.WaitForSeconds(endTime);

            this.gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.transform.TryGetComponent(out CharacterScripts enemyScripts))
                {
                    if(hitCounter > 0)
                    {
                        enemyScripts.TakeDamage(bulletDamage);
                        _ = mainCharacter.SetDmgTextTarget(collision.gameObject.transform.position, bulletDamage, bulletType);
                        hitCounter--;
                        if (hitCounter <= 0 && !bulletIsDead)
                        {
                            BulletActiveFalse();
                        }
                    }
                }
            }
        }
    }
}