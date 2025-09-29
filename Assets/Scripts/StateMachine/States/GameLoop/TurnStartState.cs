using Project.States;
using UnityEngine;

namespace Project.GameLoop.States
{
    public class TurnStartState : BaseState
    {
        public TurnStartState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            GameManager.PhaseSwitch.StartNewPhase();
            Debug.Log("Enter: Turn Start");

        }

        public override void Update()
        {
            GameManager.PhaseSwitch.CompletePhase();
            GameManager.StartNewPlayerMove();
        }

        public override void OnExit() { }
    }
}