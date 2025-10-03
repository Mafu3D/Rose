using System;
using UnityEngine;
using Project.GameLoop;

namespace Project.Combat.CombatStates
{
    public class ConcludeBattleState : State
    {
        public ConcludeBattleState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void Update(float deltaTime) { }
    }

}