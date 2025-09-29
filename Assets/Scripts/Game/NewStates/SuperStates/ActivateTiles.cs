using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class ActivateTiles : State
    {
        public ActivateTiles(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            Continue();
            return;


            List<Node> registeredNodes = GameManager.Instance.Grid.GetAllRegisteredNodes();
            foreach (Node node in registeredNodes)
            {
                node.ExecuteOnEndOfTurn();
            }
            Continue();
        }

        public override void Exit()
        {
        }

        public override void Subscribe()
        {
        }

        public override void Unsubscribe()
        {
        }

        public override void Update(float deltaTime)
        {

        }

        private void Continue()
        {
            GameManager.Instance.StartRoundEndPhase();
            StateMachine.SwitchState(new ResolvingEffects(new RoundEnd(StateMachine), StateMachine));
        }
    }
}