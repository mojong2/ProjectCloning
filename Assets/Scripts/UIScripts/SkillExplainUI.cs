using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Project_J.BulletDatas;

namespace Project_J
{
    public class SkillExplainUI : UIBase
    {
        [SerializeField] Button skillExplainButton;
        [SerializeField] TextMeshProUGUI skillTypeText;
        [SerializeField] TextMeshProUGUI skillAttackText;
        [SerializeField] TextMeshProUGUI skillCoolTimeText;
        [SerializeField] TextMeshProUGUI skillRankText;
        [SerializeField] TextMeshProUGUI skillExplainText;
        StringBuilder stringBuilder = new StringBuilder();

        SkillUI beforeSkillUI;
        private void Awake()
        {
            skillExplainButton.onClick.AddListener(() => ActiveFalse());
        }
        void ActiveFalse()
        {
            beforeSkillUI.SetActiveCraftButton(false);
            beforeSkillUI = null;
            skillExplainButton.gameObject.SetActive(false);
        }
        public void SetTextFunction(SkillUI skillUI)
        {
            if(beforeSkillUI != null)
            {
                beforeSkillUI.SetActiveCraftButton(false);
            }
            beforeSkillUI = skillUI;
            beforeSkillUI.SetActiveCraftButton(true);
            BulletData bulletData = skillUI.BulletData;
            if (bulletData != null)
            {
                skillTypeText.text = bulletData.bulletType.ToString();
                skillAttackText.text = SetText("°ø°Ý·Â : ", bulletData.damage.ToString());
                if(bulletData.bulletCount > 1)
                {
                    skillCoolTimeText.color = Color.green;
                }
                else skillCoolTimeText.color = Color.white;
                skillCoolTimeText.text = SetText("ÄðÅ¸ÀÓ : ", bulletData.coolTime.ToString(), "ÃÊ");
                skillRankText.text = SetRankText(bulletData.bulletNumber);
                skillExplainText.text = SetText("", bulletData.bulletExplain);
            }
        }
        string SetText(string text1, string text2, string text3 = "")
        {
            stringBuilder.Clear();
            stringBuilder.Append(text1).Append(text2).Append(text3);
            return stringBuilder.ToString();
        }
        string SetRankText(int k)
        {
            return k switch
            {
                0 => "ÀÏ¹Ý",
                1 => "Èñ±Í",
                _ => "À¯¹°",
            };
        }
    }
}