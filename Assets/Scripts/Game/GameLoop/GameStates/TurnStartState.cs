using System.Collections.Generic;
using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.GameLoop
{
    public class TurnStartState : State
    {
        public TurnStartState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            // Cell heroCell = GameManager.Player.HeroNode.CurrentCell;
            // List<Node> registeredNodes;
            // if (GameManager.Grid.TryGetNodesRegisteredToCell(heroCell, out registeredNodes))
            // {
            //     foreach (Node node in registeredNodes)
            //     {
            //         foreach (GameplayEffectStrategy effect in node.NodeData.OnTurnStartStrategies)
            //         {
            //             GameManager.EffectQueue.AddEffect(effect);
            //         }
            //     }
            // }

            foreach (Node node in GameManager.Grid.GetAllRegisteredNodes())
            {
                foreach (GameplayEffectStrategy effect in node.NodeData.OnTurnStartStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }
        }

        public override void Update(float time)
        {
            if (TimeInState > GameManager.Instance.MinTimeBetweenPhases)
            {
                GameManager.StartNewPlayerMove();
                StateMachine.SwitchState(new TurnStartResolveState("Turn Start Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}