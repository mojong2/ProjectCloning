using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Project_J.BulletDatas;

namespace Project_J
{
    public class GameManager : MonoBehaviour
    {
        Vector3 spawnPos1 = new Vector3(-2,-0.8f,0);
        Vector3 spawnPos2 = new Vector3(-0.52f, -0.8f, 0);
        Vector3 spawnPos3 = new Vector3(0.64f, -0.8f, 0);
        Vector3 spawnPos4 = new Vector3(2, -0.8f, 0);

        const int roundTime = 20;
        const int bossRoundTime = 60;

        public Action<int> OnMoneyPlus;
        public Action<int> OnMoneyTextChanged;
        public Action<int> OnMagicCostChanged;
        public Action<bool> OnMoneyBuyMagic;
        public Action<bool> OnMoneyBuyUpgrade;

        public Action<int> OnGemPlus;
        public Action<int> OnGemTextChanged;
        public Action<int> OnGemBuyUpgrade;

        public Action<int> OnMagicPlus;
        public Action<int> OnMagicTextChanged;

        public Action OnBossSpawn;

        public Action<int> OnRankUnitGet;

        public Action OnSpawnFriends;

        [Header("Main Settings")]
        [SerializeField] MainCharacter mainCharacter;

        [Header("Bullet Settings")]
        [SerializeField] BulletDatas[] magicBulletDatas = new BulletDatas[3];
        public BulletDatas[] MagicBulletDatas => magicBulletDatas;

        [Header("Round Settings")]
        [SerializeField] RoundDatas roundDatas;
        [SerializeField] RoundDatas hauntingDatas;
        [SerializeField] int roundNumber;

        [Header("Friends Settings")]
        [SerializeField] FriendsScripts[] friendsScripts = new FriendsScripts[2];
        FriendsScripts friends1;
        FriendsScripts friends2;

        bool bossSpawn;
        bool gameOver;
        public bool GameOver
        {
            get { return gameOver; }
            set { gameOver = value; }
        }
        public Action<int> OnUpgradeComplete;

        public Action<int> OnRoundIsChange;
        public Action<int> OnTimerUpdated;
        public Action<int> OnResultPrint;

        public Action<string> OnHauntTimerUpdated;
        public Action<string> OnHauntLimitTimerUpdated;
        public Action<int> OnHauntStart;
        public Action OnHauntAble;

        bool startHaunt;
        bool hauntFinish;
        [Header("Coin Settings")]
        [SerializeField] int magicSpawnCost;
        [SerializeField] int upgradeCost; 
        [SerializeField] int currentCoin;
        bool coinChanging;

        [Header("Gem Settings")]
        [SerializeField] int currentGem;
        bool gemChanging;

        [Header("Magic Settings")]
        [SerializeField] int currentMagic;
        public Action<BulletData> OnMagicSpawn;
        bool magicChanging;

        [Header("Enemy Settings")]
        [SerializeField] EnemyScripts enemyScripts;
        [SerializeField] GameObject enemyZip;

        [SerializeField] BossScripts bossScripts;
        [SerializeField] GameObject bossZip;

        Queue<EnemyScripts> enemyQueue = new();
        Queue<BossScripts> bossQueue = new();

        public int CurrentMagic
        {
            get { return currentMagic; }
            set { currentMagic = value; }
        }
        public int CurrentCoin
        {
            get { return currentCoin; }
            set 
            { 
                currentCoin = value;
                CheckCostCanBuy(magicSpawnCost, upgradeCost, currentCoin);
            }
        }
        public int CurrentGem
        {
            get { return currentGem; }
            set { 
                currentGem = value;
                OnGemBuyUpgrade?.Invoke(currentGem);
            }
        }
        private void Awake()
        {
            friends1 = Instantiate(friendsScripts[0]);
            friends2 = Instantiate(friendsScripts[1]);
            friends1.gameObject.SetActive(false);
            friends2.gameObject.SetActive(false);
            OnRankUnitGet += RankUnitGet;
            magicSpawnCost = 25;
            InstantiateScripts.InstantiateObject(enemyScripts, 50, enemyZip, enemyQueue);
            InstantiateScripts.InstantiateObject(bossScripts, 5, bossZip, bossQueue);
            
        }
        private void OnEnable()
        {
            OnHauntStart += StartHaunt;
        }
        private void Start()
        {
            StartCoroutine(GameStartFunction());
            StartCoroutine(HauntingFunction());
            StartCoroutine(StartHauntFunction());
        }

        public void SpawnFriendsFunction(int index)
        {
            if (index == 0)
            {
                friends1.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                friends1.transform.position = new Vector3(-1.64f, 2.33f, 0);
                BulletScript[] bulletScripts = friends1.GetComponentsInChildren<BulletScript>();

                foreach (BulletScript bulletScript in bulletScripts)
                {
                    bulletScript.SetMainCharacter(mainCharacter);
                }
                friends1.gameObject.SetActive(true);
            }
            else
            {
                friends2.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                friends2.transform.position = new Vector3(-1.64f, 1.755f, 0);
                BulletScript[] bulletScripts = friends2.GetComponentsInChildren<BulletScript>();

                foreach (BulletScript bulletScript in bulletScripts)
                {
                    bulletScript.SetMainCharacter(mainCharacter);
                }
                friends2.gameObject.SetActive(true);
            }
        }
        void StartHaunt(int number)
        {
            hauntFinish = false;
            startHaunt = true;
            GetEnemyOut(0, number);
        }
        IEnumerator StartHauntFunction()
        {
            float timer = 0.0f;
            while (this != null && !GameOver)
            {
                if (startHaunt)
                {
                    if (timer > 40.0f)
                    {
                        OnResultPrint?.Invoke(1);
                        mainCharacter.SetDeadState();
                        break;
                    }
                    else 
                    {
                        timer += Time.deltaTime;
                        int remainingTime = Mathf.CeilToInt(40 - timer - 1);
                        OnHauntLimitTimerUpdated?.Invoke(remainingTime.ToString());
                    }
                    if (hauntFinish)
                    {
                        timer = 0.0f;
                        startHaunt = false;
                        hauntFinish = false;
                    }
                }
                yield return null;
            }

        }
        IEnumerator HauntingFunction()
        {
            float timer = 60.0f;
            int intTimer = 60;
            while (this != null && !GameOver)
            {
                if (timer <= 0.0f)
                {
                    if (intTimer >= -1)
                    {
                        intTimer = -2;
                        OnHauntAble?.Invoke();
                    }
                    if (hauntFinish)
                    {
                        timer = 60.0f;
                        intTimer = 60;
                    }
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (timer < intTimer)
                    {
                        intTimer--;
                        if(intTimer < 0)
                        {
                            OnHauntTimerUpdated?.Invoke("!");
                        }
                        else OnHauntTimerUpdated?.Invoke(intTimer.ToString());
                    }
                }
                yield return null;
            }
        }
        IEnumerator GameStartFunction()
        {
            float timer = 0.0f;
            float spawnTime = 0;
            int spawnNumber = 0;
            int maxSpawnNumber = 7;
            bool bossRound = false;
            float limitTime = roundTime;
            OnRoundIsChange?.Invoke(roundNumber + 1);
            while (!gameOver)
            {
                if(timer > limitTime)
                {
                    timer = 0;
                    spawnTime = 0;
                    spawnNumber = 0;
                    if (bossRound)
                    {
                        if (bossSpawn) // 게임 졌습니다.
                        {
                            OnResultPrint?.Invoke(1);
                            mainCharacter.SetDeadState();
                            break;
                        }
                        else if(roundNumber == 29)
                        {
                            OnResultPrint?.Invoke(0);
                            mainCharacter.SetIdleState();
                            break;
                        }
                        int reward = (int)((roundNumber+1) * 0.1f);
                        UseGem(-reward);
                        maxSpawnNumber = 7;
                        bossRound = false;
                        limitTime = roundTime;
                    }
                    else 
                    {
                        int tmp = (int)(roundNumber * 0.1f);
                        int reward = tmp * 10 + 20;
                        UseCoin(-reward);
                    }
                    roundNumber++;
                    if((roundNumber+1) % 10 == 0)
                    {
                        maxSpawnNumber = 0;
                        bossRound = true;
                        limitTime = bossRoundTime;
                    }
                    OnRoundIsChange?.Invoke(roundNumber + 1);
                }
                else
                {
                    timer += Time.deltaTime;
                    if(spawnTime < timer && spawnNumber <= maxSpawnNumber)
                    {
                        spawnTime+=2.0f;
                        spawnNumber++;
                        if(bossRound)
                            GetEnemyOut(roundNumber, 1);
                        else
                            GetEnemyOut(roundNumber, 0);
                    }
                    if(bossRound)
                    {
                        if(!bossSpawn)
                        {
                            timer += 99;
                        }
                    }
                    int remainingTime = Mathf.CeilToInt(limitTime - timer - 1);
                    OnTimerUpdated?.Invoke(remainingTime);
                }

                yield return null;
            }
            gameOver = true;
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene(0);
        }
        async void GetEnemyOut(int round, int bossCheck)
        {
            if (bossCheck == 1)
            {
                if (bossQueue.Count == 0) return;
                bossSpawn = true;
                BossScripts enemy = bossQueue.Dequeue();
                enemy.SetStat(roundDatas.roundZip[round], this, true);
                enemy.transform.position = spawnPos1;
                enemy.gameObject.SetActive(true);
                OnBossSpawn?.Invoke();
                await UniTask.WaitUntil(() => (enemy != null && !enemy.gameObject.activeSelf) || gameOver);
                if(gameOver)
                {
                    enemy.gameObject.SetActive(false);
                }
                else
                {
                    int reward = (int)((round + 1) * 0.1f);
                    UseGem(-reward);
                    bossSpawn = false;
                }
                bossQueue.Enqueue(enemy);
            }
            else if(bossCheck >= 2)
            {
                if (bossQueue.Count == 0) return;
                BossScripts enemy = bossQueue.Dequeue();
                enemy.SetStat(hauntingDatas.roundZip[bossCheck - 2], this, false);
                enemy.transform.position = spawnPos1;
                enemy.gameObject.SetActive(true);
                OnBossSpawn?.Invoke();
                await UniTask.WaitUntil(() => (enemy != null && !enemy.gameObject.activeSelf) || gameOver);
                if (gameOver)
                {
                    enemy.gameObject.SetActive(false);
                }
                else
                {
                    if(enemy.Reward >= 100)
                    {
                        UseCoin(-enemy.Reward);
                    }
                    else UseGem(-enemy.Reward);
                    hauntFinish = true;
                }
                bossQueue.Enqueue(enemy);
            }
            else
            {
                if (enemyQueue.Count == 0) return;
                EnemyScripts enemy = enemyQueue.Dequeue();
                enemy.SetStat(roundDatas.roundZip[round], this);
                enemy.transform.position = spawnPos1;
                enemy.gameObject.SetActive(true);
                await UniTask.WaitUntil(() => (enemy != null && !enemy.gameObject.activeSelf) || gameOver);
                if (gameOver)
                {
                    enemy.gameObject.SetActive(false);
                }
                else
                {
                    UseCoin(-3);
                    enemyQueue.Enqueue(enemy);
                }
            }
        }
        void CheckCostCanBuy(int magicCost, int upgradeCost, int curCoin)
        {
            bool CostCheck = curCoin >= magicCost;
            OnMoneyBuyMagic?.Invoke(CostCheck);
            CostCheck = curCoin >= upgradeCost;
            OnMoneyBuyUpgrade?.Invoke(CostCheck);
        }
        public void BuyMagicFunction()
        {
            if (UseCoin(magicSpawnCost))
            {
                magicSpawnCost++;
                OnMagicCostChanged?.Invoke(magicSpawnCost);
                RandomUnitGet();
            }
        }
        void RandomUnitGet()
        {
            int R = UnityEngine.Random.Range(0, 3);
            int R1 = UnityEngine.Random.Range(0, 2);

            BulletData bulletData = mainCharacter.AcquireBullet((BulletType)R, R1);

            OnMagicSpawn?.Invoke(bulletData);
            OnSpawnFriends?.Invoke();
            UseMagic(-1);
        }
        void RankUnitGet(int rank)
        {
            int R = UnityEngine.Random.Range(0, 3);
            int R1 = rank;

            BulletData bulletData = mainCharacter.AcquireBullet((BulletType)R, R1);

            OnMagicSpawn?.Invoke(bulletData);
            OnSpawnFriends?.Invoke();
            UseMagic(-1);
        }
        bool UseMagic(int magic)
        {
            if (CurrentMagic < magic) return false;
            if (CurrentMagic >= 25) return false;

            OnMagicPlus?.Invoke(magic);

            if (!magicChanging)
            {
                StartCoroutine(MagicChangeFunction(magic));
            }
            else
            {
                CurrentMagic -= magic;
            }
            return true;
        }
        public void UpgradeFunction(ref int cost, int magicType)
        {
            if (UseGem(cost))
            {
                OnUpgradeComplete?.Invoke(magicType);
                cost++;
            }
        }
        public void GetMagicFunction(int cost)
        {
            if (UseCoin(cost))
            {
                if (cost == 1)
                {
                    RankUnitGet(2);
                }
                else if (cost == 3)
                {
                    RankUnitGet(3);
                }
                else RankUnitGet(4);
            }
        }
        bool UseGem(int gem)
        {
            if (CurrentGem < gem) return false;

            OnGemPlus?.Invoke(gem);

            if (!gemChanging)
            {
                StartCoroutine(GemChangeFunction(gem));
            }
            else
            {
                CurrentGem -= gem;
            }
            return true;
        }
        bool UseCoin(int coin) // coin을 마이너스로 받는다면 돈을 받는 것 또한 가능하다.
        {
            if (CurrentCoin < coin) return false; // 돈이 없구나. 없다는 UI출력 후 변화는 주지 않는다.

            OnMoneyPlus?.Invoke(coin); //돈이 있다면 우선 추가하는 이벤트부터 시작!

            if (!coinChanging) // 변화의 시작
            {
                StartCoroutine(CoinChangeFunction(coin));
            }
            else // 만약 변화중이라면 기존에 실행중이던 코루틴에서 currentCoin이 바뀌었으므로 실행될 것이다.
            {
                CurrentCoin -= coin;
            }
            return true;
        }
        IEnumerator MagicChangeFunction(int useMagic)
        {
            magicChanging = true; // 변화 시작@!

            int coinTmp = CurrentMagic;
            CurrentMagic -= useMagic;

            while (coinTmp != CurrentMagic)
            {
                if (coinTmp > CurrentMagic)
                {
                    coinTmp--;
                }
                else
                {
                    coinTmp++;
                }
                OnMagicTextChanged?.Invoke(coinTmp);
                yield return null;
            }
            // 최종 값 출력.
            OnMagicTextChanged?.Invoke(coinTmp);
            magicChanging = false; // 변화는 끝났다.
        }
        IEnumerator CoinChangeFunction(int useCoin)
        {
            coinChanging = true; // 변화 시작@!

            int coinTmp = CurrentCoin;
            CurrentCoin -= useCoin;

            while(coinTmp != CurrentCoin)
            {
                if (coinTmp > CurrentCoin)
                {
                    coinTmp--;
                }
                else
                {
                    coinTmp++;
                }
                OnMoneyTextChanged?.Invoke(coinTmp);
                yield return null;
            }
            // 최종 값 출력.
            OnMoneyTextChanged?.Invoke(coinTmp);
            coinChanging = false; // 변화는 끝났다.
        }
        IEnumerator GemChangeFunction(int useGem)
        {
            gemChanging = true; // 변화 시작@!

            int gemTmp = CurrentGem;
            CurrentGem -= useGem;

            while (gemTmp != CurrentGem)
            {
                if (gemTmp > CurrentGem)
                {
                    gemTmp--;
                }
                else
                {
                    gemTmp++;
                }
                OnGemTextChanged?.Invoke(gemTmp);
                yield return null;
            }
            // 최종 값 출력.
            OnGemTextChanged?.Invoke(gemTmp);
            gemChanging = false; // 변화는 끝났다.
        }
    }
}