using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Project_J
{
    public class CoinPlusUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI coinPlusText;

        public void CoinPlusTextChange(int coin)
        {
            coinPlusText.text = coin.ToString();
        }
    }
}