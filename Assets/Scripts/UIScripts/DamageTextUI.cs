using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using static Project_J.BulletDatas;

namespace Project_J
{
    public class DamageTextUI : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] TextMeshProUGUI dmgText;
        [SerializeField] MainCharacter mainCharacter;

        public async Task SetStat(Vector3 pos, int dmg, BulletType bulletType)
        {
            transform.position = pos + Vector3.down * 0.5f;
            dmgText.text = FormatDamage(dmg);
            switch (bulletType)
            {
                case BulletType.Ice:
                    dmgText.color = Color.blue;
                    break;
                case BulletType.Thunder:
                    dmgText.color = Color.yellow;
                    break;
                default:
                    dmgText.color = Color.red;
                    break;
            }
            this.gameObject.SetActive(true);
            await UniTask.WaitForSeconds(0.5f);
            this.gameObject.SetActive(false);
        }
        private string FormatDamage(int damage)
        {
            if (damage >= 1000)
            {
                dmgText.fontSize = 3;
                float damageInK = damage / 1000f; 
                return $"{damageInK:0.0}k";
            }
            else
            {
                dmgText.fontSize = 2;
                return damage.ToString();
            }
        }
    }
}