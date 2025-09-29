using Project.States;
using UnityEngine;

namespace Project.GameLoop.States
{
    public class EndOfTurnState : BaseState
    {
        public EndOfTurnState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            GameManager.PhaseSwitch.StartNewPhase();
            Debug.Log("Enter: End Of Turn");
        }

        public override void Update()
        {
            GameManager.StartNewRound();
            GameManager.PhaseSwitch.CompletePhase();
        }

        public override void OnExit() { }
    }
}