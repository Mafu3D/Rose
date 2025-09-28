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
        public event Action OnChoice1Input;
        public event Action OnChoice2Input;
        public event Action OnChoice3Input;


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

        public void OnChoice1(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnChoice1Input?.Invoke();
        }

        public void OnChoice2(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnChoice2Input?.Invoke();
        }

        public void OnChoice3(InputAction.CallbackContext context)
        {
            if (!context.performed) { return; }
            OnChoice3Input?.Invoke();
        }
    }
}