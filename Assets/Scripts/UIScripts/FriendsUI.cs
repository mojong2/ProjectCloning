using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Project_J
{
    public class FriendsUI : UIBase
    {
        [SerializeField] MainCharacter mainCharacter;
        [SerializeField] string[] whatsFriendsNeed = new string[2];
        [SerializeField] TextMeshProUGUI[] whatsFriendsText = new TextMeshProUGUI[2];

        [SerializeField] Button[] buttonZip = new Button[2];

        [SerializeField] Sprite[] buttonSprite = new Sprite[2];

        StringBuilder stringBuilder = new StringBuilder();
        private void Start()
        {
            ButtonSetting();
        }
        private void OnEnable()
        {
            GameManager.OnSpawnFriends += CanSpawnFriends;
        }
        private void OnDisable()
        {
            GameManager.OnSpawnFriends -= CanSpawnFriends;
        }
        void ButtonSetting()
        {
            for (int i = 0; i < buttonZip.Length; i++)
            {
                int k = i;
                buttonZip[k].onClick.AddListener(() => SpawnFriends(k));
            }
        }
        void SpawnFriends(int index)
        {
            buttonZip[index].gameObject.SetActive(false);
            GameManager.SpawnFriendsFunction(index);
        }
        void CanSpawnFriends()
        {
            for (int i = 0; i < whatsFriendsNeed.Length; i++)
            {
                int number = mainCharacter.CheckFriendSpawnAble(whatsFriendsNeed[i]);
                stringBuilder.Clear();
                stringBuilder.Append(number).Append("/3");
                whatsFriendsText[i].text = stringBuilder.ToString();
                if (buttonZip[i].TryGetComponent(out Image image))
                {
                    if (number >= 3)
                    {
                        buttonZip[i].interactable = true;
                        image.sprite = buttonSprite[1];
                    }
                    else
                    {
                        buttonZip[i].interactable = false;
                        image.sprite = buttonSprite[0];
                    }
                }
            }
        }
    }
}