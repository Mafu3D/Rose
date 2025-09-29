using Project.States;
using UnityEngine;

namespace Project.States
{
    public class RoundStartState : BaseState
    {
        public RoundStartState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            // GameManager.PhaseSwitch.StartNewPhase();

        }

        public override void Update()
        {
            GameManager.StartNewTurn();
            // GameManager.PhaseSwitch.CompletePhase();
        }

        public override void OnExit() { }
    }
}