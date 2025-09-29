using Project.States;
using UnityEngine;

namespace Project.GameLoop.States
{
    public class RoundStartState : BaseState
    {
        public RoundStartState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            GameManager.PhaseSwitch.StartNewPhase();
            Debug.Log("Enter: Round Start");

        }

        public override void Update()
        {
            GameManager.PhaseSwitch.CompletePhase();
            GameManager.StartNewTurn();
        }

        public override void OnExit() { }
    }
}