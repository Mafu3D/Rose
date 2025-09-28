using UnityEngine;
using Project.Decks;
using Project.States;
using Project.UI.MainUI;
using System.Collections.Generic;
using System;
namespace Project.GameStates
{
    public class ResolveCardDraw : SubState
    {
        public ResolveCardDraw(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        bool isSingleChoice = false;

        public override void Enter()
        {
            isSingleChoice = false;
            DrawEncounterCard();
        }

        public override void Exit() { }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += OnProceed;
            GameManager.Instance.Player.InputReader.OnChoice1Input += ChooseOption1;
            GameManager.Instance.Player.InputReader.OnChoice2Input += ChooseOption2;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= OnProceed;
            GameManager.Instance.Player.InputReader.OnChoice1Input -= ChooseOption1;
            GameManager.Instance.Player.InputReader.OnChoice2Input -= ChooseOption2;
        }

        private void ChooseOption2()
        {
            if (!isSingleChoice) {
                GameManager.Instance.ActiveCardChoice.ChooseItem(1);
            }
        }

        private void ChooseOption1()
        {
            if (!isSingleChoice) {
                GameManager.Instance.ActiveCardChoice.ChooseItem(0);
            }
        }

        public override void Update(float deltaTime) { }

        private void DrawEncounterCard()
        {
            List<Card> cards = GameManager.Instance.EncounterDeck.DrawMultiple(2);
            if (cards.Count == 0)
            {
                MoveToNextState();
                return;
            }
            foreach (Card card in cards)
            {
                if (card.CardType == CardType.Monster)
                {
                    Card monsterCard = GameManager.Instance.MonsterDeck.Draw();
                    if (monsterCard == null)
                    {
                    }
                    else
                    {
                        isSingleChoice = true;
                        cards = new List<Card> { monsterCard };
                        break;
                    }
                }
            }

            Choice<Card> cardChoice = new Choice<Card>(cards, ExecuteCard);
            GameManager.Instance.StartNewCardChoice(cardChoice);
        }

        private void OnProceed()
        {
            if (isSingleChoice)
            {
                GameManager.Instance.ActiveCardChoice.ChooseItem(0);
            }
        }

        private void ExecuteCard(Card card) {
            card.Execute();
            GameManager.Instance.EndCardChoice();
            MoveToNextState();
        }

        private void MoveToNextState()
        {
            StateMachine.SwitchState(new EndOfRound(new ResolvingEffects(StateMachine), StateMachine));
        }
    }
}