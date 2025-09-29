using System;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class NoState : State
    {
        public NoState(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

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