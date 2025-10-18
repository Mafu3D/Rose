using System;
using UnityEngine;
using Project.States;

namespace Project.Combat.CombatStates
{
    public class BattleFinishedState : State
    {
        public BattleFinishedState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            GameManager.BattleManager.ActiveBattle.CloseBattle();
            if (GameManager.Player.HeroTile.Character.Attributes.GetAttributeValue(Attributes.AttributeType.Health) <= 0)
            {
                GameManager.HandleGameOver();
            }
        }

        public override void OnExit()
        {
        }

        public override void Update(float deltaTime) { }
    }

}