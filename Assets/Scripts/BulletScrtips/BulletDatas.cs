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
            Fire = 0,    // 불꽃
            Ice = 1,      // 얼음
            Thunder = 2, // 전기
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
            [Header("총알 데이터")]
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

            [Header("총알 속성")]
            public int damage; // 데미지는?
            public int hitCount; // 몇명까지 때릴꺼니?
            public float bulletSpeed; // 속도는?
            public float maxDistance; // 어디 까지 갈꺼니?
            public float minusCoolTime; // 쿨타임 어디까지 감소해봤니?
            public float coolTime; // 쿨타임은 몇초니?
            public float coolTimer; //이건 진짜 가는 쿨타임머
            public float endTime; // 언제 사라질거니?
            public int spawnNumber; // 스폰 넘버

            public bool splashDeadAttack; // 너 총알이 끝나고 스플이 있는 공격이니?
            public bool pungAnimation; // 너 펑 애니메 있어?
            public BulletType bulletType; // 너의 타입은?
        }
        public List<BulletData> bulletZip = new List<BulletData>();
    }
    
}