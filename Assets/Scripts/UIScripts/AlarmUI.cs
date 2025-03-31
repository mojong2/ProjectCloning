using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public class AlarmUI : UIBase
    {
        [Header("Round UI")]
        [SerializeField] Animator animator;
        [SerializeField] TextMeshProUGUI roundText;
        [SerializeField] TextMeshProUGUI rewardText;
        [SerializeField] Image rewardImage; 
        [SerializeField] Sprite[] rewardSprites = new Sprite[2];

        [Header("Result UI")]
        [SerializeField] GameObject resultUI;
        [SerializeField] TextMeshProUGUI resultText;

        StringBuilder stringBuilder = new StringBuilder();
        string[] resultContext = { "You Win!", "You Lose..." };

        private void OnEnable()
        {
            GameManager.OnRoundIsChange += ChangeRoundText;
            GameManager.OnResultPrint += PrintResultUI;
        }
        private void OnDisable()
        {
            GameManager.OnRoundIsChange -= ChangeRoundText;
            GameManager.OnResultPrint -= PrintResultUI;
        }
        void PrintResultUI(int result)
        {
            animator.SetTrigger("Lose");
            resultText.text = resultContext[result];
        }
        void ChangeRoundText(int currentRound)
        {
            animator.SetTrigger("Replay");
            int tmp = (int)(currentRound * 0.1f);
            stringBuilder.Clear();
            stringBuilder.Append("Round ").Append(currentRound.ToString());
            roundText.text = stringBuilder.ToString();
            if (currentRound % 10 == 0) // 보스라운드입니다.
            {
                rewardImage.sprite = rewardSprites[1];
                rewardText.text = (tmp + 1).ToString();
            }
            else
            {
                rewardImage.sprite = rewardSprites[0];
                int k = tmp * 10 + 20;
                rewardText.text = k.ToString();
            }
        }

    }
}