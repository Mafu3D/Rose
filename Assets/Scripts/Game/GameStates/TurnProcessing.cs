using System.Collections.Generic;
using Project;
using Project.GameNode;
using Project.Grid;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class TurnProcessing : SubState
    {
        private List<Node> nodesToProcess;
        private int currentProcessingNode = 0;

        public TurnProcessing(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter()
        {
            nodesToProcess = GameManager.Instance.Grid.GetNodesRegisteredToCell(GameManager.Instance.Hero.CurrentCell);
        }

        public override void Exit() { }

        public override void Subscribe() { }

        public override void Unsubscribe() { }

        public override void Update(float deltaTime)
        {
            Status status = ProcessTurn();
            if (status == Status.Success)
            {
                currentProcessingNode = 0;
                EndTurn();
            }
        }

        private Status ProcessTurn()
        {
            while (currentProcessingNode < nodesToProcess.Count)
            {
                Status status = nodesToProcess[currentProcessingNode].Process();
                if (status != Status.Success)
                {
                    return status;
                }
                nodesToProcess[currentProcessingNode].Reset();
                currentProcessingNode++;
            }
            return Status.Success;
        }

        private void EndTurn()
        {
            GameManager.Instance.IncrementTurn();
            StateMachine.SwitchState(new PlayerMove(new GameRunning(StateMachine), StateMachine));
        }
    }
}