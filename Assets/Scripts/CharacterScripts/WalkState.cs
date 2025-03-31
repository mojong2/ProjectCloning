using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class WalkState : ICharacterState
    {
        public CharacterStateEnum characterStateEnum => CharacterStateEnum.Walk;
        public void EnterState(CharacterScripts character)
        {
            character.AnimatorFunction("Walk", true);
        }
        public void UpdateState(CharacterScripts character)
        {
            character.WalkFunction(Vector3.up);
        }

        public void ExitState(CharacterScripts character)
        {
            character.WalkFunction(Vector3.zero);
            character.AnimatorFunction("Walk", false);
        }
    }
}