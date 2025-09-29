

using System;
using UnityEngine;

namespace Project.States
{
    public abstract class BaseState : IState
    {
        protected GameManager GameManager;
        public string Name => name;
        private string name;
        public event Action Exit;

        public BaseState(string name, GameManager gameManager)
        {
            this.name = name;
            this.GameManager = gameManager;
        }

        public virtual void OnEnter() { }
        public virtual void Update() { }
        public virtual void OnExit() { }
    }
}