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
    public class TurnResolving : State
    {
        private List<Node> nodesToResolve = new();
        private int resolvingNodeIndex = 0;

        public Node CurrentResolvingNode
        {
            get
            {
                if (nodesToResolve.Count > 0)
                {
                    return nodesToResolve[resolvingNodeIndex];
                }
                return null;
            }
        }

        public TurnResolving(StateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            nodesToResolve = GameManager.Instance.Grid.GetNodesRegisteredToCell(GameManager.Instance.Hero.CurrentCell);
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
            Status status = ResolveTurn();
            if (status == Status.Complete)
            {
                resolvingNodeIndex = 0;
                EndTurn();
            }
        }

        private Status ResolveTurn()
        {
            while (resolvingNodeIndex < nodesToResolve.Count)
            {
                Status status = nodesToResolve[resolvingNodeIndex].OnTurnResolve();
                if (status != Status.Complete)
                {
                    return status;
                }
                // nodesToResolve[resolvingNodeIndex].Reset();
                resolvingNodeIndex++;
            }
            return Status.Complete;
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