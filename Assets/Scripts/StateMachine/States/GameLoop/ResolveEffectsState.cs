using Project.States;
using UnityEngine;

namespace Project.GameLoop.States
{
    public class ResolveEffectsState : BaseState
    {
        public ResolveEffectsState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            // GameManager.PhaseSwitch.StartNewPhase();
        }

        public override void Update()
        {
            // GameManager.PhaseSwitch.CompletePhase();
        }

        public override void OnExit() { }
    }
}