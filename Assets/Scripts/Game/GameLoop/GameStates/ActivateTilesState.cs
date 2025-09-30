using System.Collections.Generic;
using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.GameLoop
{
    public class ActivateTilesState : State
    {
        public ActivateTilesState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            Cell heroCell = GameManager.Player.HeroNode.CurrentCell;
            List<Node> registeredNodes;
            if (GameManager.Grid.TryGetNodesRegisteredToCell(heroCell, out registeredNodes))
            {
                foreach (Node node in registeredNodes)
                {
                    foreach (GameplayEffectStrategy effect in node.NodeData.OnActivateStrategies)
                    {
                        GameManager.EffectQueue.AddEffect(effect);
                    }
                }
                StateMachine.SwitchState(new ActivateTilesResolveState("Activate Tiles Resolve", StateMachine, GameManager));
            }
            else
            {
                StateMachine.SwitchState(new DrawCardState("Draw Card", StateMachine, GameManager));
            }
        }

        public override void Update(float time)
        {
        }

        public override void OnExit() { }
    }
}