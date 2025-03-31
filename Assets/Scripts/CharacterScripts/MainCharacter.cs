using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static Project_J.BulletDatas;

namespace Project_J // 협업 진행 시 같은 이름의 class가 생기지 않게 하기 위해 사용합니다.
{
    public class MainCharacter : CharacterScripts
    {
        const int dmgTextCount = 50;
        const int bulletZipMaxCount = 240;

        [Header("Damage Text Settings")]
        [SerializeField] GameObject dmgTextZip;
        [SerializeField] DamageTextUI dmgText;
        Queue<DamageTextUI> dmgTextQueue = new();

        [Header("Bullet Settings")]
        [SerializeField] GameObject bulletZip;
        [SerializeField] BulletScript bulletScript;

        Dictionary<BulletType, List<BulletData>> bulletInventory = new();

        [SerializeField] int[] upgradeStat = new int[3]; 

        Queue<BulletScript> bulletQueue = new();

        public int CheckFriendSpawnAble(string input)
        {
            int answer = 0;
            for (int i = 0; i < input.Length; i++)
            {
                int type = int.Parse(input[i].ToString());

                if (i % 2 == 0) 
                {
                    int grade = int.Parse(input[i + 1].ToString());
                    BulletType bulletType = (BulletType)type;
                    if(bulletInventory[bulletType][grade].bulletCount > 0)
                    {
                        answer++;
                    }
                }
            }
            return answer;
        }

        private void Awake()
        {
            InitializeMagicFunction();
            InstantiateScripts.InstantiateObject(dmgText, dmgTextCount, dmgTextZip, dmgTextQueue);
            InstantiateScripts.InstantiateObject(bulletScript, bulletZipMaxCount, bulletZip, bulletQueue);
            SetIdleState();
        }
        private void Start()
        {
            gameManager.OnUpgradeComplete += UpgradeFunction;
            SetStat(5000);
        }
        void UpgradeFunction(int magicType)
        {
            if(!(magicType == 0 || magicType == 4))
            {
                upgradeStat[magicType - 1]++;
            }
        }
        void SetStat(int hp)
        {
            MaxHpValue = hp;
            HpValue = hp;
        }
        public async Task SetDmgTextTarget(Vector3 targetPos, int dmg, BulletType bulletType)
        {
            DamageTextUI damageTextUI = dmgTextQueue.Dequeue();
            await damageTextUI.SetStat(targetPos, dmg, bulletType);
            dmgTextQueue.Enqueue(damageTextUI);
        }
        public override void SetDeadState()
        {
            base.SetDeadState();
            gameManager.GameOver = true;
        }

        void InitializeMagicFunction()
        {
            InitializeMagicZero(BulletType.Fire, gameManager.MagicBulletDatas[0]);

            InitializeMagicZero(BulletType.Thunder, gameManager.MagicBulletDatas[1]);

            InitializeMagicZero(BulletType.Ice, gameManager.MagicBulletDatas[2]);
        }
        void InitializeMagicZero(BulletType bulletType, BulletDatas bulletDatas)
        {
            bulletInventory[bulletType] = new List<BulletData>();
            foreach (var bullet in bulletDatas.bulletZip)
            {
                BulletData bulletData = bullet.Clone();
                bulletInventory[bulletType].Add(bulletData);
            }
        }
        public BulletData AcquireBullet(BulletType bulletType, int bulletNumber)
        {
            if (bulletInventory.ContainsKey(bulletType))
            {
                int bulletCount = bulletInventory[bulletType][bulletNumber].bulletCount;
                if (bulletCount == -1) // 만약 처음 왔다? 코루틴 시작!!!
                {
                    bulletInventory[bulletType][bulletNumber].bulletCount = 0;
                    StartCoroutine(MagicAttackFunction(bulletInventory[bulletType][bulletNumber]));
                }
                else
                {
                    if (bulletCount <= 3)
                    {
                        float baseCoolTime = gameManager.MagicBulletDatas[(int)bulletType].bulletZip[bulletNumber].coolTime;
                        float reduceCoolTime = gameManager.MagicBulletDatas[(int)bulletType].bulletZip[bulletNumber].minusCoolTime;
                        float resultCoolTime = baseCoolTime - reduceCoolTime * bulletCount;
                        bulletInventory[bulletType][bulletNumber].coolTime = resultCoolTime;
                    }
                }
                bulletInventory[bulletType][bulletNumber].bulletCount++;
                return bulletInventory[bulletType][bulletNumber];
            }
            else return null;
        }
        IEnumerator MagicAttackFunction(BulletData bulletData)
        {
            while (!characterState.characterStateEnum.Equals(CharacterStateEnum.Dead))
            {
                if (bulletData.bulletCount != 0)
                {
                    if (bulletData.coolTimer >= bulletData.coolTime)
                    {
                        bulletData.coolTimer = 0.0f;
                        AnimatorFunction("Attack", true);
                        for (int i = 0; i < bulletData.spawnNumber; i++)
                        {
                            BulletScript bullet = bulletQueue.Dequeue();
                            SetBulletStat(bullet, bulletData);
                        }
                    }
                    else bulletData.coolTimer += Time.deltaTime;
                }
                else
                {
                    if(bulletData.coolTimer != 0.0f)
                    {
                        bulletData.coolTimer = 0.0f;
                    }
                }
                yield return null;
            }
        }
        async void SetBulletStat(BulletScript bulletScript, BulletData bulletData)
        {
            bulletScript.SetStat(bulletData, upgradeStat[(int)(bulletData.bulletType)], this);
            Vector3 vector3;
            if (bulletData.bulletSpeed < 0)
            {
                if(bulletData.bulletSpeed <= -1.0f)
                {
                    float plus = Random.Range(0.5f, bulletData.yPostion);
                    vector3 = this.transform.position + (Vector3.right * bulletData.xPostion + Vector3.down * plus);
                }
                else vector3 = this.transform.position + (Vector3.right * bulletData.xPostion + Vector3.down * bulletData.yPostion);
            }
            else
            {
                vector3 = this.transform.position + (Vector3.right * bulletData.xPostion + Vector3.down * bulletData.yPostion);
            }
            Quaternion rotation = Quaternion.Euler(0, 0, bulletData.zDegree);
            bulletScript.transform.SetPositionAndRotation(vector3, rotation);
            bulletScript.gameObject.SetActive(true);
            await UniTask.WaitUntil(() => bulletScript != null && bulletScript.gameObject.activeSelf == false);
            bulletQueue.Enqueue(bulletScript);
        }
    }
}