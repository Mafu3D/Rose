using System;
using Project.Decks;
using Project.GameTiles;
using Project.GameplayEffects;
using Project.States;
using UnityEngine;

namespace Project.GameLoop
{
    public class DrawCardState : State
    {
        public DrawCardState(string name,
                                StateMachine stateMachine,
                                GameManager gameManager) : base(name, stateMachine, gameManager) { }

        public override void OnEnter()
        {
            Debug.Log($"Enter: {Name}");
            GameManager.Player.InputReader.OnProceedInput += Proceed;
            GameManager.Player.InputReader.OnChoice1Input += ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input += ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input += ChooseOptionThree;

            GameManager.TileDrawManager.DrawTiles(3);
        }

        private void ChooseOptionThree()
        {
            GameManager.TileDrawManager.ActiveTileChoice.ChooseItem(2);
        }

        private void ChooseOptionTwo()
        {
            GameManager.TileDrawManager.ActiveTileChoice.ChooseItem(1);
        }

        private void ChooseOptionOne()
        {
            GameManager.TileDrawManager.ActiveTileChoice.ChooseItem(0);
        }

        private void Proceed()
        {
            if (GameManager.TileDrawManager.ActiveTileChoice.NumberOfChoices == 1)
            {
                GameManager.TileDrawManager.ActiveTileChoice.ChooseItem(0);
            }
        }

        public override void Update(float time)
        {
            if (!GameManager.TileDrawManager.TileChoiceIsActive)
            {
                foreach (Tile tile in GameManager.Grid.GetAllRegisteredTiles())
                {
                    foreach (GameplayEffectStrategy effect in tile.TileData.OnDrawCardStrategies)
                    {
                        GameManager.EffectQueue.AddEffect(effect);
                    }
                }
                StateMachine.SwitchState(new DrawCardResolveState("Draw Card Resolve", StateMachine, GameManager));
            }
        }

        public override void OnExit()
        {
            GameManager.Player.InputReader.OnProceedInput -= Proceed;
            GameManager.Player.InputReader.OnChoice1Input -= ChooseOptionOne;
            GameManager.Player.InputReader.OnChoice2Input -= ChooseOptionTwo;
            GameManager.Player.InputReader.OnChoice3Input -= ChooseOptionThree;
        }
    }
}