using System;
using System.Collections.Generic;
using Project;
using Project.Decks;
using Project.GameNode;
using Project.Grid;
using Project.States;
using UnityEngine;

namespace Project.GameStates
{
    public class ResolveCardDraw : SubState
    {
        Card card;
        public ResolveCardDraw(State superState, StateMachine stateMachine) : base(superState, stateMachine) { }

        public override void Enter()
        {
            if (GameManager.Instance.Grid.AreNodesRegisteredToCell(GameManager.Instance.Hero.CurrentCell))
            {
                MoveToNextState();
            }
            else
            {
                DrawEncounterCard();
            }
        }

        public override void Exit() { }

        public override void Subscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput += OnProceed;
        }

        public override void Unsubscribe()
        {
            GameManager.Instance.Player.InputReader.OnProceedInput -= OnProceed;
        }

        public override void Update(float deltaTime) { }

        private void DrawEncounterCard()
        {
            card = GameManager.Instance.EncounterDeck.DrawCard();
            if (card == null)
            {
                MoveToNextState();
                return;
            }
            MainUI.Instance.DisplayCard(card);
        }

        private void OnProceed()
        {
            MainUI.Instance.DestroyDisplayedCard();
            if (card != null)
            {
                card.Execute();
            }
            MoveToNextState();
        }

        private void MoveToNextState()
        {
            StateMachine.SwitchState(new TurnProcessing(new GameRunning(StateMachine), StateMachine));
        }
    }
}