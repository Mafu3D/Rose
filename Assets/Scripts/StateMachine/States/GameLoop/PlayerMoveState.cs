using Project.States;
using UnityEngine;

namespace Project.GameLoop.States
{
    public class PlayerMoveState : BaseState
    {
        public PlayerMoveState(string name, GameManager gameManager) : base(name, gameManager) { }

        public override void OnEnter()
        {
            GameManager.PhaseSwitch.StartNewPhase();
            Debug.Log("Enter: Player Move");

        }

        public override void Update()
        {
            GameManager.StartNewEndOfTurn();
            GameManager.PhaseSwitch.CompletePhase();
        }

        public override void OnExit() { }
    }
}