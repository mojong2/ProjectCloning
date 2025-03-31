using System.Collections;
using System.Collections.Generic;
using System.Text;
using Project_J;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Project_J.BulletDatas;

namespace Project_J
{
    public class SkillUI : UIBase
    {
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] TextMeshProUGUI numberText;
        [SerializeField] GameObject canUpgrade;
        [SerializeField] Image canCraftButton;
        [SerializeField] Slider skillCoolTimeSlider;

        [SerializeField] BulletType bulletType;
        [SerializeField] Animator animator;
        [SerializeField] Button skillButton;
        public Button SkillButton => skillButton;

        [SerializeField] int magicLevel;
        [SerializeField] int magicNumber;

        BulletData bulletData;
        public BulletData BulletData
        {
            get { return bulletData; }
            set
            {
                bulletData = value;
            }
        }
        StringBuilder stringBuilder = new StringBuilder();
        public int MagicNumber
        {
            get { return magicNumber; }
            set
            {
                magicNumber = value;
                if (magicNumber >= 3)
                {
                    skillCoolTimeSlider.gameObject.SetActive(true);
                    canUpgrade.SetActive(true);
                    canCraftButton.color = Color.blue;
                }
                else if(magicNumber == 0)
                {
                    skillCoolTimeSlider.value = 0;
                    skillCoolTimeSlider.gameObject.SetActive(false);
                    canUpgrade.SetActive(false);
                    canCraftButton.color = Color.gray;
                }
                else
                {
                    skillCoolTimeSlider.gameObject.SetActive(true);
                    canUpgrade.SetActive(false);
                    canCraftButton.color = Color.gray;
                }

                animator.SetTrigger("Replay");
                numberText.text = magicNumber.ToString();
            }
        }
        public void SetActiveCraftButton(bool check)
        {
            canCraftButton.gameObject.SetActive(check);
        }
        void CraftButton()
        {
            if(MagicNumber >= 3)
            {
                if(magicLevel == 1)
                {
                    GameManager.OnRankUnitGet?.Invoke(magicLevel);
                    bulletData.bulletCount -= 3;
                    MagicNumber = bulletData.bulletCount;
                    GameManager.CurrentMagic -= 3;
                }
                else
                {

                }
            }
        }

        private void Awake()
        {
            if(canCraftButton.TryGetComponent(out Button button))
            {
                button.onClick.AddListener(() => CraftButton());
            }
            canUpgrade.SetActive(false);
            stringBuilder.Clear();
            stringBuilder.Append("Lv. ").Append(magicLevel.ToString());
            levelText.text = stringBuilder.ToString();
            if(magicLevel <= 2 && magicLevel >= 0)
            {
                bulletData = GameManager.MagicBulletDatas[(int)bulletType].bulletZip[magicLevel - 1];
                StartCoroutine(CoolTimerSliderFunction());
            }
        }
        IEnumerator CoolTimerSliderFunction()
        {
            float coolTimeSav = bulletData.coolTime;
            float ratio = 1.0f / coolTimeSav;
            while (!GameManager.GameOver)
            {
                if(bulletData.coolTime != coolTimeSav)
                {
                    coolTimeSav = bulletData .coolTime;
                    ratio = 1.0f / coolTimeSav;
                }
                if(MagicNumber > 0)
                {
                    skillCoolTimeSlider.value = bulletData.coolTimer * ratio;
                }
                yield return null;
            }
        }
    }
}