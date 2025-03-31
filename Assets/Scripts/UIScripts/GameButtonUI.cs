using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public class GameButtonUI : UIBase
    {

        [Header("Magic Spawn Settings")]
        [SerializeField] Button spawnButton;
        [SerializeField] GameObject canBuyAlarm;
        [SerializeField] TextMeshProUGUI magicCostText;

        [Header("Button Settings")]
        [SerializeField] Button hauntingButton;
        [SerializeField] CanvasGroup hauntingUI;
        [SerializeField] TextMeshProUGUI hauntingTimeText;

        [Header("Friends Settings")]
        [SerializeField] Button friendsButton;
        [SerializeField] CanvasGroup friendsUI;

        [Header("Reinforce Settings")]
        [SerializeField] Button reinforceButton;
        [SerializeField] CanvasGroup reinforceUI;

        [Header("Mining Settings")]
        [SerializeField] Button miningButton;
        [SerializeField] CanvasGroup miningUI;

        Button buttonSav;
        CanvasGroup uISav;
        [SerializeField] Animator canvasUIAnima;

        
        private void OnEnable()
        {
            SetSpawnButton();
        }
        private void OnDisable()
        {
            GameManager.OnMagicCostChanged -= MagicCostChanged;
            GameManager.OnMoneyBuyMagic -= MoneyCanBuyMagic;
            GameManager.OnHauntTimerUpdated -= HauntTimerChange;
        }

        void SetSpawnButton()
        {
            spawnButton.onClick.AddListener(() => GameManager.BuyMagicFunction());
            GameManager.OnMagicCostChanged += MagicCostChanged;
            GameManager.OnMoneyBuyMagic += MoneyCanBuyMagic;
            GameManager.OnHauntTimerUpdated += HauntTimerChange;

            hauntingButton.onClick.AddListener(() => CanvasUIAnimation(hauntingButton, hauntingUI));
            friendsButton.onClick.AddListener(() => CanvasUIAnimation(friendsButton, friendsUI));
            reinforceButton.onClick.AddListener(() => CanvasUIAnimation(reinforceButton, reinforceUI));
            miningButton.onClick.AddListener(() => CanvasUIAnimation(miningButton, miningUI));
        }
        void HauntTimerChange(string time)
        {
            hauntingTimeText.text = time;
        }
        void CanvasUIAnimation(Button btn, CanvasGroup obj)
        {
            if(btn == buttonSav)
            {
                obj.alpha = 0;
                obj.interactable = false;
                obj.blocksRaycasts = false;
                canvasUIAnima.SetBool("Up", false);
                canvasUIAnima.SetBool("Down", true);
                buttonSav = null;
                uISav = null;
            }
            else
            {
                if (uISav != null)
                {
                    uISav.alpha = 0;
                    uISav.interactable = false;
                    uISav.blocksRaycasts = false;
                }
                obj.alpha = 1;
                obj.interactable = true;
                obj.blocksRaycasts = true;
                if (!canvasUIAnima.GetBool("Up"))
                {
                    canvasUIAnima.SetBool("Down", false);
                    canvasUIAnima.SetBool("Up", true);
                }
                buttonSav = btn;
                uISav = obj;
            }
        }
        void MoneyCanBuyMagic(bool Check)
        {
            canBuyAlarm.SetActive(Check);
        }
        void MagicCostChanged(int changeCost)
        {
            magicCostText.text = changeCost.ToString();
        }
    }
}