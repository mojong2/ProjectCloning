using System.Collections;
using System.Collections.Generic;
using Project_J;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public class ReinforceUI : UIBase
    {

        [SerializeField] Button[] buttonZip = new Button[5];

        [SerializeField] Sprite[] buttonSprite = new Sprite[2];

        [SerializeField] int[] upgradeGemNeed = new int[5];

        [SerializeField] TextMeshProUGUI[] upgradeText = new TextMeshProUGUI[5];

        private void Start()
        {
            ButtonSetting();
        }
        void ButtonSetting()
        {
            for (int i = 0; i < buttonZip.Length; i++) 
            {
                int k = i;
                buttonZip[k].onClick.AddListener(() => UpgradeButton(k));
            }
        }
        void UpgradeButton(int index)
        {
            GameManager.UpgradeFunction(ref upgradeGemNeed[index], index);
            upgradeText[index].text = upgradeGemNeed[index].ToString();
        }
        private void OnEnable()
        {
            GameManager.OnGemBuyUpgrade += UpgradeAble;
        }
        private void OnDisable()
        {
            GameManager.OnGemBuyUpgrade -= UpgradeAble;
        }
        void UpgradeAble(int currenGem)
        {
            for (int i = 0; i < upgradeGemNeed.Length; i++)
            {
                if (upgradeGemNeed[i] <= currenGem)
                {
                    buttonZip[i].interactable = true;
                    if (buttonZip[i].TryGetComponent(out Image image))
                    {
                        image.sprite = buttonSprite[1];
                    }
                }
                else
                {
                    buttonZip[i].interactable = false;
                    if (buttonZip[i].TryGetComponent(out Image image))
                    {
                        image.sprite = buttonSprite[0];
                    }
                }
            }
        }
    }
}