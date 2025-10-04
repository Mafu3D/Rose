using System;

namespace Project.States
{
    public interface IState
    {
        string Name { get; }
        void OnEnter();
        void Update(float time);
        void OnExit();
    }
}