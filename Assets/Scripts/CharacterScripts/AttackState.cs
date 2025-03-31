using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class AttackState : ICharacterState
    {
        float timer = 0.0f;
        float attackCoolTime = 0.0f;
        public CharacterStateEnum characterStateEnum => CharacterStateEnum.Attack;
        public void EnterState(CharacterScripts character)
        {
            timer = 0.0f;
            attackCoolTime = character.AttackCoolTime;
        }
        public void UpdateState(CharacterScripts character)
        {
            if (timer >= attackCoolTime)
            {
                timer = 0.0f;
                character.AnimatorFunction("Attack", true);
            }
            else timer += Time.deltaTime;
        }
        public void ExitState(CharacterScripts character)
        {

        }
    }
}