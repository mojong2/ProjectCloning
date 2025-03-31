using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Project_J.BulletDatas;

namespace Project_J
{
    public class MagicSkillUI : UIBase
    {
        [SerializeField] SkillUI[] skillUIZip = new SkillUI[6];
        [SerializeField] SkillExplainUI skillUIExplain;
        [SerializeField] BulletType bulletType;
        private void OnEnable()
        {
            GameManager.OnMagicSpawn += MagicSpawnFunction;
        }
        private void Awake()
        {
            ButtonInitialize();
        }
        void ButtonInitialize()
        {
            for (int i = 0; i < skillUIZip.Length; i++)
            {
                int k = i;
                skillUIZip[k].SkillButton.onClick.AddListener(() => PressSkillUI(skillUIZip[k]));
            }
        }
        void MagicSpawnFunction(BulletData bulletData)
        {
            if(bulletType == bulletData.bulletType)
            {
                skillUIZip[bulletData.bulletNumber].BulletData = bulletData;
                skillUIZip[bulletData.bulletNumber].MagicNumber = bulletData.bulletCount;
            }
        }
        void PressSkillUI(SkillUI skillUI)
        {
            skillUIExplain.gameObject.SetActive(true);
            skillUIExplain.SetTextFunction(skillUI);
        }
    }
}