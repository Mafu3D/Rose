using System;
using Project.Decks;
using Project.GameTiles;
using Project.GameplayEffects;
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

            GameManager.CardDrawManager.DrawCards(GameManager.EncounterDeck, 2);
        }

        private void ChooseOptionThree()
        {
            GameManager.CardDrawManager.ActiveCardChoice.ChooseItem(2);
        }

        private void ChooseOptionTwo()
        {
            GameManager.CardDrawManager.ActiveCardChoice.ChooseItem(1);
        }

        private void ChooseOptionOne()
        {
            GameManager.CardDrawManager.ActiveCardChoice.ChooseItem(0);
        }

        private void Proceed()
        {
            if (GameManager.CardDrawManager.ActiveCardChoice.NumberOfChoices == 1)
            {
                GameManager.CardDrawManager.ActiveCardChoice.ChooseItem(0);
            }
        }

        public override void Update(float time)
        {
            if (!GameManager.CardDrawManager.CardChoiceIsActive)
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