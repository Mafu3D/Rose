using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.PlayerSystem.Input
{
    /// <summary>
    /// Reads the input from the controls.
    /// </summary>
    public class InputReader : MonoBehaviour, Controls.IMainActions
    {
        public Vector2 MovementValue { get; private set; }

        // Events (TODO: Clean these up with new controls!)
        public event Action OnMoveInput;
        public event Action OnProceedInput;
        public event Action<int> OnNumInput;
        public event Action OnExitInput;



        private Controls controls;

        void Awake()
        {
            controls = new Controls();
            controls.Main.SetCallbacks(this);
            controls.Main.Enable();
        }

        private void OnDestroy()
        {
            controls.Main.Disable();
        }


        public void OnMove(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
            OnMoveInput?.Invoke();
        }

        public void OnProceed(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnProceedInput?.Invoke();
        }

        public void OnOne(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(1);
        }

        public void OnTwo(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(2);
        }

        public void OnThree(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(3);
        }

        public void OnFour(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(4);
        }

        public void OnFive(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(5);
        }

        public void OnSix(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(6);
        }

        public void OnSeven(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(7);
        }

        public void OnEight(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(8);
        }

        public void OnNine(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(9);
        }

        public void OnZero(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnNumInput?.Invoke(0);
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnExitInput?.Invoke();
        }
    }
}