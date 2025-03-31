using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Project_J.BulletDatas;

namespace Project_J
{
    [CreateAssetMenu(fileName = "RoundData", menuName = "ScriptableObjects/RoundData")]
    public class RoundDatas : ScriptableObject
    {
        [System.Serializable]
        public struct RoundData
        {
            [Header("라운드 특성")]
            public int roundNumber; // 라운드 번호
            public int monsterHealth; // 몬스터 체력
            public int monsterAttack; // 몬스터 공격력
            public float monsterAttackSpeed; // 몬스터 공격 속도
            public int reward;
        }
        public List<RoundData> roundZip = new List<RoundData>();
    }
}