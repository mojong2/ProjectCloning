using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public class MiningUI : UIBase
    {
        [SerializeField] Button[] buttonZip = new Button[3];

        [SerializeField] Sprite[] buttonSprite = new Sprite[2];

        [SerializeField] int[] miningCost = new int[3];

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
            GameManager.GetMagicFunction(miningCost[index]);
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
            for (int i = 0; i < miningCost.Length; i++)
            {
                if (miningCost[i] <= currenGem)
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