using System;

namespace Project.GameLoop
{
    public interface IState
    {
        string Name { get; }
        void OnEnter();
        void Update(float time);
        void OnExit();
    }
}