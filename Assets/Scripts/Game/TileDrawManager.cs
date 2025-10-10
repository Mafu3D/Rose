using System;
using System.Collections.Generic;
using Project.Decks;
using Project.GameTiles;

namespace Project.Core
{
    public class TileDrawManager
    {
        public event Action OnNewTileDrawEvent;
        public event Action OnConcludeTileDrawEvent;

        public bool TileChoiceIsActive => ActiveTileChoice != null;
        public Choice<TileData> ActiveTileChoice;

        public void DrawTiles(int amount)
        {
            if (TileChoiceIsActive) return;

            List<TileData> tiles = GameManager.Instance.TileDeck.DrawMultiple(amount);
            if (tiles.Count == 0) return;

            // ActiveTileChoice = new Choice<TileData>(tiles, ResolveChoice);
            OnNewTileDrawEvent?.Invoke();
        }

        public bool TryGetActiveTileChoice(out Choice<TileData> choice)
        {
            choice = ActiveTileChoice;
            return TileChoiceIsActive;
        }

        public void MakeChoice(int choiceIndex)
        {
            if (choiceIndex < ActiveTileChoice.NumberOfChoices)
            {
                ActiveTileChoice.ChooseItem(choiceIndex);
            }
        }

        private void ResolveChoice(TileData chosen, List<TileData> notChosen)
        {
            // Make tile and activate it
            TileFactory.Instance.CreateTile(chosen, GameManager.Instance.Hero.CurrentCell.Center);

            GameManager.Instance.TileDeck.AddToRemaining(notChosen, true);
            ActiveTileChoice = null;
            OnConcludeTileDrawEvent?.Invoke();
        }
    }
}