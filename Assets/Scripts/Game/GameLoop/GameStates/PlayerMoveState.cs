using System;
using System.Collections.Generic;
using Project.GameNode;
using Project.GameplayEffects;
using UnityEngine;

namespace Project.GameLoop
{
    public class PlayerMoveState : State
    {
        public PlayerMoveState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        float timeInState;

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            GameManager.Player.InputReader.OnProceedInput += Proceed;

            if (GameManager.Player.HeroNode.MovesRemaining == 0)
            {
                foreach (Node node in GameManager.Grid.GetAllRegisteredNodes())
                {
                    foreach (GameplayEffectStrategy effect in node.NodeData.OnPlayerMoveEndStrategies)
                    {
                        GameManager.EffectQueue.AddEffect(effect);
                    }
                }
                Proceed();
            }
        }

        public override void Update(float time)
        {
            timeInState += Time.deltaTime;
            if (timeInState > GameManager.MinTimeBetweenPlayerMoves)
            {
                Vector2 movementInput = GameManager.Player.InputReader.MovementValue;
                if (movementInput != Vector2.zero)
                {
                    GameManager.Player.HeroNode.Move(movementInput);
                    GameManager.OnPlayerMove();
                    Resolve();
                }
            }
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= Proceed;
        }

        private void Proceed()
        {
            StateMachine.SwitchState(new PlayerMoveEndResolveState("Player Move End Resolve", StateMachine, GameManager));
        }

        private void Resolve()
        {
            foreach (Node node in GameManager.Grid.GetAllRegisteredNodes())
            {
                foreach (GameplayEffectStrategy effect in node.NodeData.OnPlayerMoveStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }

            Cell heroCell = GameManager.Player.HeroNode.CurrentCell;
            List<Node> registeredNodes;
            if (GameManager.Grid.TryGetNodesRegisteredToCell(heroCell, out registeredNodes))
            {
                foreach (Node node in registeredNodes)
                {
                    foreach (GameplayEffectStrategy effect in node.NodeData.OnPlayerEnterStrategies)
                    {
                        GameManager.EffectQueue.AddEffect(effect);
                    }
                }
            }
            StateMachine.SwitchState(new PlayerMoveResolveState("Player Move Resolve", StateMachine, GameManager));
        }
    }
}