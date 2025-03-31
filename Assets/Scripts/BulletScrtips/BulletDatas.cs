using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEditor.U2D.Animation;
using UnityEngine;

namespace Project_J
{
    [CreateAssetMenu(fileName = "NewBulletData", menuName = "ScriptableObjects/BulletData")]
    public class BulletDatas : ScriptableObject
    {
        public enum BulletType
        {
            Fire = 0,    // �Ҳ�
            Ice = 1,      // ����
            Thunder = 2, // ����
        }
        [System.Serializable]
        public class BulletData
        {
            public BulletData Clone()
            {
                BulletData newItem = new BulletData
                {
                    bulletName = this.bulletName,
                    bulletNumber = this.bulletNumber,
                    animator = this.animator,
                    damage = this.damage,
                    hitCount = this.hitCount,
                    coolTime = this.coolTime,
                    bulletSpeed = this.bulletSpeed,
                    maxDistance = this.maxDistance,
                    bulletType = this.bulletType,
                    bulletCount = this.bulletCount,
                    splashDeadAttack = this.splashDeadAttack,
                    endTime = this.endTime,
                    circleSize = this.circleSize,
                    zDegree = this.zDegree,
                    xPostion = this.xPostion,
                    yPostion = this.yPostion,
                    spawnNumber = this.spawnNumber,
                    pungAnimation = this.pungAnimation,
                    minusCoolTime = this.minusCoolTime,
                    bulletExplain = this.bulletExplain,
                };
                return newItem;
            }
            [Header("�Ѿ� ������")]
            public string bulletName;
            public int bulletNumber;
            public AnimatorController animator;
            public int bulletCount;
            public float circleSize;

            public float zDegree;
            public float xPostion;
            public float yPostion;

            [TextArea(2, 3)]
            public string bulletExplain;

            [Header("�Ѿ� �Ӽ�")]
            public int damage; // ��������?
            public int hitCount; // ������ ��������?
            public float bulletSpeed; // �ӵ���?
            public float maxDistance; // ��� ���� ������?
            public float minusCoolTime; // ��Ÿ�� ������ �����غô�?
            public float coolTime; // ��Ÿ���� ���ʴ�?
            public float coolTimer; //�̰� ��¥ ���� ��Ÿ�Ӹ�
            public float endTime; // ���� ������Ŵ�?
            public int spawnNumber; // ���� �ѹ�

            public bool splashDeadAttack; // �� �Ѿ��� ������ ������ �ִ� �����̴�?
            public bool pungAnimation; // �� �� �ִϸ� �־�?
            public BulletType bulletType; // ���� Ÿ����?
        }
        public List<BulletData> bulletZip = new List<BulletData>();
    }
    
}