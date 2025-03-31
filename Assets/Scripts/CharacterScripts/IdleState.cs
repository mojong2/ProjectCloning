using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project_J
{
    public class IdleState : ICharacterState
    {
        public CharacterStateEnum characterStateEnum => CharacterStateEnum.Idle;
        public void EnterState(CharacterScripts character)
        {

        }
        public void UpdateState(CharacterScripts character)
        {

        }
        public void ExitState(CharacterScripts character)
        {

        }
    }
}