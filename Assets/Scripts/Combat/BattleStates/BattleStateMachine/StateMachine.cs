using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Combat.BattleStateMachine
{
    /// <summary>
    /// Base class for unit state machines. Ticks the current state every Update.
    /// </summary>
    public class StateMachine
    {
        public StateMachine() { }

        public State CurrentState;
        public State PreviousState;

        public event Action OnSwitchStateEvent;

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            CurrentState?.Update(deltaTime);
            CurrentState?.UpdateTimeInState(deltaTime);
        }

        public void SwitchState(State newState)
        {
            // Exit the previous state
            CurrentState?.Exit();
            CurrentState?.Unsubscribe();

            // Cache the previous state
            if (CurrentState != null) PreviousState = CurrentState;

            // Set the new state and its parent state
            CurrentState = newState;

            // Enter the new state
            CurrentState?.Subscribe();
            CurrentState?.Enter();

            OnSwitchStateEvent?.Invoke();
        }

    }
}