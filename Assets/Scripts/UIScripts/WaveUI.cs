using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Project_J
{
    public class WaveUI : UIBase
    {
        [SerializeField] TextMeshProUGUI roundText;

        StringBuilder stringBuilder = new StringBuilder();
        private void OnEnable()
        {
            GameManager.OnRoundIsChange += ChangeRoundText;
        }
        private void OnDisable()
        {
            GameManager.OnRoundIsChange -= ChangeRoundText;
        }
        void ChangeRoundText(int currentRound)
        {
            stringBuilder.Clear();
            stringBuilder.Append("Round ").Append(currentRound.ToString());
            roundText.text = stringBuilder.ToString();
        }
    }
}