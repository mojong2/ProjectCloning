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
            [Header("���� Ư��")]
            public int roundNumber; // ���� ��ȣ
            public int monsterHealth; // ���� ü��
            public int monsterAttack; // ���� ���ݷ�
            public float monsterAttackSpeed; // ���� ���� �ӵ�
            public int reward;
        }
        public List<RoundData> roundZip = new List<RoundData>();
    }
}