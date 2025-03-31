using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project_J
{
    public enum CharacterStateEnum
    {
        Idle, Attack, Dead, Walk
    }
    public interface ICharacterState
    {
        CharacterStateEnum characterStateEnum { get; }
        void EnterState(CharacterScripts character);
        void UpdateState(CharacterScripts character);
        void ExitState(CharacterScripts character);
    }
    public class CharacterScripts : MonoBehaviour
    {
        [Header("UI Settings")]
        [SerializeField] protected GameManager gameManager;
        [SerializeField] protected Slider hpSlider;

        [Header("Character Settings")]
        [SerializeField] protected Animator animator;
        [SerializeField] Rigidbody2D rigidbody2D;

        [Header("Character Stats")]
        [SerializeField] protected int hpValue;
        [SerializeField] protected int maxHpValue;
        [SerializeField] protected float maxHpRatio;
        [SerializeField] protected float moveSpeed;
        [SerializeField] protected float attackCoolTime;
        [SerializeField] protected int attackDamage;

        public float AttackCoolTime => attackCoolTime;
        public int HpValue
        {
            get { return hpValue; }
            set
            {
                hpValue = value;
                hpSlider.value = maxHpRatio * hpValue;
            }
        }
        public int MaxHpValue
        {
            get { return maxHpValue; }
            set
            {
                maxHpValue = value;
                maxHpRatio = 1.0f / maxHpValue;
            }
        }

        protected ICharacterState characterState;

        static WalkState walkState = new();
        static DeadState deadState = new();
        static AttackState attackState = new();
        static IdleState idleState = new();

        private void Update()
        {
            characterState?.UpdateState(this);
        }
        public virtual void TakeDamage(int dmg)
        {
            HpValue -= dmg;
        }
        public void WalkFunction(Vector3 vector3)
        {
            rigidbody2D.velocity = vector3 * moveSpeed;
        }
        public void AnimatorFunction(string anima, bool triggerbool)
        {
            if (anima == "Attack") animator.SetTrigger(anima);
            else
            {
                animator.SetBool(anima, triggerbool);
            }
        }
        public void SetIdleState()
        {
            SetState(idleState);
        }
        public void SetWalkState()
        {
            SetState(walkState);
        }
        public virtual void SetDeadState()
        {
            SetState(deadState);
        }
        public void SetAttackState()
        {
            SetState(attackState);
        }
        public void SetState(ICharacterState newState)
        {
            characterState?.ExitState(this);
            characterState = newState;
            characterState.EnterState(this);
        }
    }
}
