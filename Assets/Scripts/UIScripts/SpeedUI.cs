using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public class SpeedUI : MonoBehaviour
    {
        [SerializeField] Button speedButton;
        [SerializeField] TextMeshProUGUI speedText;

        private void Awake()
        {
            speedButton.onClick.AddListener(() => ChangeSpeed());
        }
        void ChangeSpeed()
        {
            if (Time.timeScale <= 1.0f)
            {
                Time.timeScale = 3.0f;
            }
            else Time.timeScale = 1.0f;
            speedText.text = Time.timeScale.ToString();
        }

    }
}
