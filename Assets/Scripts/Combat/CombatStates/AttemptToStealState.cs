using System;
using UnityEngine;
using Project.GameLoop;
using Project.States;

namespace Project.Combat.CombatStates
{
    public class AttemptToStealState : State
    {
        public AttemptToStealState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void Update(float deltaTime) { }
    }

}