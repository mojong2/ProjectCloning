using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class DeadState : ICharacterState
    {
        public CharacterStateEnum characterStateEnum => CharacterStateEnum.Dead;
        public void EnterState(CharacterScripts character)
        {
            character.tag = "Dead";
            character.AnimatorFunction("Dead", true);
        }
        public void UpdateState(CharacterScripts character)
        {

        }
        public void ExitState(CharacterScripts character)
        {
            character.AnimatorFunction("Dead", false);
        }
    }

}