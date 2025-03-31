using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public class HauntingUI : UIBase
    {

        [SerializeField] GameButtonUI gameButtonUI;

        [SerializeField] Button[] bossZip = new Button[2];

        [SerializeField] Sprite[] bossSprite = new Sprite[2];

        private void OnEnable()
        {
            GameManager.OnHauntAble += HauntingAble;
        }
        private void OnDisable()
        {
            GameManager.OnHauntAble -= HauntingAble;
        }
        private void Awake()
        {
            ButtonSpawnFunction();
        }
        void ButtonSpawnFunction()
        {
            for (int i = 0; i < bossZip.Length; i++)
            {
                int k = i;
                bossZip[i].onClick.AddListener(() => HauntStart(k));
            }
        }
        void HauntStart(int number)
        {
            GameManager.OnHauntStart?.Invoke(number + 2);
            HauntingDisable();
        }
        void HauntingAble()
        {
            for(int i = 0; i < bossZip.Length; i++)
            {
                if (bossZip[i].TryGetComponent(out Image image))
                {
                    bossZip[i].interactable = true;
                    image.sprite = bossSprite[1];
                }
            }
        }
        void HauntingDisable()
        {
            for (int i = 0; i < bossZip.Length; i++)
            {
                if (bossZip[i].TryGetComponent(out Image image))
                {
                    bossZip[i].interactable = false;
                    image.sprite = bossSprite[0];
                }
            }
        }
    }
}