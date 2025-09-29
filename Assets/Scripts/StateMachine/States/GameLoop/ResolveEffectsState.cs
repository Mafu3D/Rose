using Project.States;
using UnityEngine;

namespace Project.GameLoop.States
{
    public class ResolveEffectsState : BaseState
    {
        public ResolveEffectsState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            GameManager.PhaseSwitch.StartNewPhase();
            Debug.Log("Enter: Resolve Effects");
        }

        public override void Update()
        {
            GameManager.PhaseSwitch.CompletePhase();
        }

        public override void OnExit() { }
    }
}