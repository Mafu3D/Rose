using Project.States;
using UnityEngine;

namespace Project.States
{
    public class EndOfTurnState : BaseState
    {
        public EndOfTurnState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            // GameManager.PhaseSwitch.StartNewPhase();
        }

        public override void Update()
        {
            // GameManager.PhaseSwitch.CompletePhase();
            GameManager.StartNewRound();
        }

        public override void OnExit() { }
    }
}