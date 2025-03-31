using System.Collections;
using System.Collections.Generic;
using System.Text;
using Project_J;
using TMPro;
using UnityEngine;

namespace Project_J
{
    public class TimerUI : UIBase
    {
        [SerializeField] TextMeshProUGUI timeText;
        [SerializeField] GameObject timePlus;
        [SerializeField] TextMeshProUGUI timePlusText;

        StringBuilder stringBuilder = new StringBuilder();
        private void OnEnable()
        {
            GameManager.OnTimerUpdated += ChangeTimerText;
        }
        private void OnDisable()
        {
            GameManager.OnTimerUpdated -= ChangeTimerText;
        }
        void ChangeTimerText(int time)
        {
            if (time <= 5)
            {
                timePlusText.text = time.ToString();
                timePlus.SetActive(true);
                timeText.color = Color.red;
            }
            else
            {
                timeText.color = Color.white;
                timePlus.SetActive(false);
            }
         
            timeText.text = FormatTime(time);
        }
        private string FormatTime(int totalSeconds)
        {
            int minutes = totalSeconds / 60; 
            int seconds = totalSeconds % 60;  
            return string.Format("{0:D2}:{1:D2}", minutes, seconds); 
        }
    }
}