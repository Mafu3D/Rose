using Project.GameTiles;
using Project.GameplayEffects;
using UnityEngine;
using Project.States;

namespace Project.GameLoop
{
    public class RoundStartState : State
    {
        public RoundStartState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");

            foreach(Attributes.Attribute attribute in GameManager.Instance.Player.HeroTile.Character.Attributes.GetAllAttributes())
            {
                attribute.TickAttributeModifiers();
            }

            foreach (Tile tile in GameManager.Grid.GetAllRegisteredTiles())
            {
                foreach (GameplayEffectStrategy effect in tile.TileData.OnRoundStartStrategies)
                {
                    GameManager.EffectQueue.AddEffect(effect);
                }
            }
        }

        public override void Update(float time)
        {
            if (TimeInState > GameManager.Instance.MinTimeBetweenPhases)
            {
                StateMachine.SwitchState(new RoundStartResolveState("Round Start Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit() { }
    }
}