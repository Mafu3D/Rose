using System;
using System.Collections.Generic;
using Project.Decks;

namespace Project.Core
{
    public class CardDrawManager
    {
        public event Action OnNewCardDrawEvent;
        public event Action OnConcludeCardDrawEvent;

        public bool CardChoiceIsActive => ActiveCardChoice != null;
        public Choice<Card> ActiveCardChoice;

        public void DrawCards(Deck<Card> deck, int amount)
        {
            if (CardChoiceIsActive) return;

            List<Card> cards = deck.DrawMultiple(amount);
            if (cards.Count == 0) return;

            List<Card> monsterCards;
            if (CheckForMonsters(cards, out monsterCards)) cards = monsterCards;

            // ActiveCardChoice = new Choice<Card>(cards, ResolveChoice);
            OnNewCardDrawEvent?.Invoke();
        }

        public bool TryGetActiveCardChoice(out Choice<Card> choice)
        {
            choice = ActiveCardChoice;
            return CardChoiceIsActive;
        }

        public void MakeChoice(int choiceIndex)
        {
            if (choiceIndex < ActiveCardChoice.NumberOfChoices)
            {
                ActiveCardChoice.ChooseItem(choiceIndex);
            }
        }

        private void ResolveChoice(Card chosen, List<Card> notChosen)
        {
            chosen.Execute();
            ActiveCardChoice = null;
            OnConcludeCardDrawEvent?.Invoke();
        }

        private bool CheckForMonsters(List<Card> cards, out List<Card> monsterCards)
        {
            monsterCards = new();
            foreach (Card card in cards)
            {
                if (card.CardType == CardType.Monster)
                {
                    Card monsterCard = GameManager.Instance.MonsterDeck.Draw();
                    if (monsterCard != null)
                    {
                        monsterCards.Add(monsterCard);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}