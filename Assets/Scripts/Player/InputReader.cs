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
        public event Action OnMoveInputEvent;

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
            OnMoveInputEvent?.Invoke();
        }
    }
}