using System;
using Project;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class NoPlayerInput : SubState
    {
        public NoPlayerInput(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter() { }

        public override void Exit() { }

        public override void Subscribe()
        {
        }

        public override void Unsubscribe()
        {
        }


        public override void Update(float deltaTime)
        {
        }
    }
}