using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Project_J
{
    public class ResourceUI : UIBase
    {
        const int plusCount = 20;
        [Header("Coin Settings")]
        [SerializeField] GameObject coinObject;
        [SerializeField] TextMeshProUGUI coinText;
        [SerializeField] CoinPlusUI coinPlusPrefab;
        [SerializeField] GameObject coinPlusZip;
        Queue<CoinPlusUI> coinPlusQueue = new();

        [Header("Gem Settings")]
        [SerializeField] GameObject gemObject;
        [SerializeField] TextMeshProUGUI gemText;
        [SerializeField] CoinPlusUI gemPlusPrefab;
        [SerializeField] GameObject gemPlusZip;
        Queue<CoinPlusUI> gemPlusQueue = new();

        [Header("Magic Settings")]
        [SerializeField] GameObject magicObject;
        [SerializeField] TextMeshProUGUI magicText;
        [SerializeField] CoinPlusUI magicPlusPrefab;
        [SerializeField] GameObject magicPlusZip;
        Queue<CoinPlusUI> magicPlusQueue = new();

        StringBuilder stringBuilder = new StringBuilder();

        private void Awake()
        {
            InstantiateScripts.InstantiateObject(coinPlusPrefab, plusCount, coinPlusZip, coinPlusQueue);
            InstantiateScripts.InstantiateObject(gemPlusPrefab, plusCount, gemPlusZip, gemPlusQueue);
            InstantiateScripts.InstantiateObject(magicPlusPrefab, plusCount, magicPlusZip, magicPlusQueue);
        }
        private void OnEnable()
        {
            GameManager.OnMoneyTextChanged += MoneyChange;
            GameManager.OnGemTextChanged += GemChange;
            GameManager.OnMoneyPlus += CoinPlusActive;
            GameManager.OnGemPlus += GemPlusActive;
            GameManager.OnMagicPlus += MagicPlusActive;
            GameManager.OnMagicTextChanged += MagicChange;
        }
        private void OnDisable()
        {
            GameManager.OnMoneyTextChanged -= MoneyChange;
            GameManager.OnGemTextChanged -= GemChange;
            GameManager.OnMoneyPlus -= CoinPlusActive;
            GameManager.OnGemPlus -= GemPlusActive;

            GameManager.OnMagicTextChanged -= MagicChange;
            GameManager.OnMagicPlus -= MagicPlusActive;
        }
        void MoneyChange(int coin)
        {
            coinText.text = coin.ToString();
        }
        void GemChange(int gem)
        {
            gemText.text = gem.ToString();
        }
        void MagicChange(int magic)
        {
            stringBuilder.Clear();
            stringBuilder.Append(magic.ToString()).Append("/25");
            magicText.text = stringBuilder.ToString();
        }
        async void CoinPlusActive(int coin)
        {
            CoinPlusUI coinPlusUI = coinPlusQueue.Dequeue();
            coinPlusUI.transform.position = coinPlusPrefab.transform.position;
            coinPlusUI.CoinPlusTextChange(-coin);
            coinPlusUI.gameObject.SetActive(true);
            await UniTask.WaitForSeconds(1.0f);
            coinPlusQueue.Enqueue(coinPlusUI);
            coinPlusUI.gameObject.SetActive(false);
        }
        async void GemPlusActive(int gem)
        {
            CoinPlusUI gemPlusUI = gemPlusQueue.Dequeue();
            gemPlusUI.transform.position = gemPlusPrefab.transform.position;
            gemPlusUI.CoinPlusTextChange(-gem);
            gemPlusUI.gameObject.SetActive(true);
            await UniTask.WaitForSeconds(1.0f);
            gemPlusQueue.Enqueue(gemPlusUI);
            gemPlusUI.gameObject.SetActive(false);
        }
        async void MagicPlusActive(int magic)
        {
            CoinPlusUI magicPlusUI = magicPlusQueue.Dequeue();
            magicPlusUI.transform.position = magicPlusPrefab.transform.position;
            magicPlusUI.CoinPlusTextChange(-magic);
            magicPlusUI.gameObject.SetActive(true);
            await UniTask.WaitForSeconds(1.0f);
            magicPlusQueue.Enqueue(magicPlusUI);
            magicPlusUI.gameObject.SetActive(false);
        }
    }
}