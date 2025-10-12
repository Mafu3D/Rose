using System.Collections.Generic;
using Project.Decks;

namespace Project.Core.GameEvents
{
    public class CardChoiceEvent : ChoiceEvent<Card>
    {
        private bool shuffleRemaining;
        Deck<Card> deck;
        public CardChoiceEvent(Deck<Card> deck, int amount, bool shuffleRemaining) : base(amount, false)
        {
            this.shuffleRemaining = shuffleRemaining;
            this.deck = deck;
        }

        public override void GenerateChoices()
        {
            List<Card> choices = deck.DrawMultiple(amount);
            if (choices.Count == 0) return;
            Choice = new Choice<Card>(choices, ResolveCallback);
        }

        public override void ChooseItem(int index)
        {
            Choice.ChooseItem(index);
        }

        protected override void ResolveCallback(List<Card> chosen, List<Card> notChosen)
        {
            foreach (Card card in chosen)
            {
                card.Execute();
            }

            if (shuffleRemaining)
            {
                deck.AddToRemaining(notChosen, true);
            }
        }

        public override void Resolve()
        {
            Choice.Resolve();
        }
    }
}