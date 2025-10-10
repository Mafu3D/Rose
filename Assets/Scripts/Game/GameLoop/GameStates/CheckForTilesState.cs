using Project.States;
using UnityEngine;

namespace Project.GameLoop
{
    public class CheckForTilesState : State
    {
        public CheckForTilesState(string name, StateMachine stateMachine, GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            // You're checking the hero cell twice!
            Cell heroCell = GameManager.Player.HeroTile.CurrentCell;
            if (GameManager.Grid.TryGetTileesRegisteredToCell(heroCell, out _))
            {
                StateMachine.SwitchState(new DrawMonsterState("Draw Monster", StateMachine, GameManager));
            }
            else
            {
                StateMachine.SwitchState(new DrawCardState("Draw Tile", StateMachine, GameManager));
            }
        }

        public override void Update(float time)
        {
        }

        public override void OnExit() { }
    }
}