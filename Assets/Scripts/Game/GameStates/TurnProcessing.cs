using System.Collections.Generic;
using Project;
using Project.Combat;
using Project.Decks;
using Project.GameNode;
using Project.Grid;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class TurnProcessing : State
    {
        private List<Node> nodesToProcess = new();
        private int currentProcessingNode = 0;

        public TurnProcessing(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            nodesToProcess = GameManager.Instance.Grid.GetNodesRegisteredToCell(GameManager.Instance.Hero.CurrentCell);
            Debug.Log("Start Turn Processing");
        }

        public override void Exit()
        {
            Debug.Log("End Turn Processing");
        }

        public override void Subscribe() {
            BattleManager.Instance.OnBattleStart += EnterCombat;
         }

        public override void Unsubscribe() {
            BattleManager.Instance.OnBattleStart -= EnterCombat;
         }

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
                Debug.Log($"Processing: {nodesToProcess[currentProcessingNode]}");
                if (status != Status.Success)
                {
                    return status;
                }
                nodesToProcess[currentProcessingNode].Reset();
                currentProcessingNode++;
            }
            return Status.Success;
        }

        private void EnterCombat()
        {
            StateMachine.SwitchState(new Combat(this, StateMachine));
        }

        private void EndTurn()
        {
            GameManager.Instance.IncrementTurn();
            StateMachine.SwitchState(new PlayerMove(new PlayerTurn(StateMachine), StateMachine));
        }
    }
}