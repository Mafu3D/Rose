using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.States
{
    /// <summary>
    /// Base class for unit state machines. Ticks the current state every Update.
    /// </summary>
    public class StateMachine
    {
        public StateMachine()
        {
        }

        private SubState _currentState;
        public SubState CurrentState
        {
            get
            {
                return _currentState;
            }
            protected set
            {
                _currentState = value;
            }
        }
        public State CurrentSuperState
        {
            get
            {
                return _currentState.SuperState;
            }
        }

        private SubState _previousState;
        public SubState PreviousState
        {
            get
            {
                return _previousState;
            }
            private set
            {
                _previousState = value;
            }
        }
        private State _previousSuperState;
        public State PreviousSuperState
        {
            get
            {
                return _previousSuperState;
            }
            private set
            {
                _previousSuperState = value;
            }
        }

        public event Action OnSwitchStateEvent;

        public void Update()
        {
            float deltaTime = Time.deltaTime;
            CurrentState?.Update(deltaTime);
            CurrentState?.SuperState?.Update(deltaTime);
            CurrentState?.UpdateTimeInState(deltaTime);
            CurrentState?.SuperState?.UpdateTimeInState(deltaTime);
        }

        public void SwitchState(SubState newState)
        {
            // Exit the previous state
            CurrentState?.Exit();
            if (CurrentState != null)
            {
                if (newState.SuperState != CurrentState.SuperState)
                {
                    CurrentState?.SuperState?.Exit();
                }
            }
            CurrentState?.Unsubscribe();
            if (CurrentState != null)
            {
                if (newState.SuperState != CurrentState.SuperState)
                {
                    CurrentState?.SuperState?.Unsubscribe();
                }
            }

            // Cache the previous state
            if (CurrentState != null)
            {
                PreviousState = CurrentState;
                PreviousSuperState = CurrentSuperState;
            }

            // Set the new state and its parent state
            CurrentState = newState;

            // Enter the new state
            CurrentState?.Subscribe();
            if (PreviousState != null)
            {
                if (newState.SuperState != PreviousState.SuperState)
                {
                    CurrentState?.SuperState?.Subscribe();
                }
            }

            CurrentState?.Enter();
            if (PreviousState != null)
            {
                if (newState.SuperState != PreviousState.SuperState)
                {
                    CurrentState?.SuperState?.Enter();
                }
            }


            // Callback
            OnSwitchState();
        }

        protected virtual void OnSwitchState()
        {
            OnSwitchStateEvent?.Invoke();
        }

    }
}