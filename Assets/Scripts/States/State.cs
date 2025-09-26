using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.States
{
    public abstract class State
    {
        protected StateMachine StateMachine;
        public State(StateMachine stateMachine)
        {
            this.StateMachine = stateMachine;
        }

        public float TimeInState { get; private set; } = 0f;
        public abstract void Enter();

        public abstract void Update(float deltaTime);

        public abstract void Exit();

        public abstract void Subscribe();

        public abstract void Unsubscribe();

        public override string ToString() { return this.GetType().Name; }

        public void UpdateTimeInState(float deltaTime) { TimeInState += deltaTime; }
    }
}