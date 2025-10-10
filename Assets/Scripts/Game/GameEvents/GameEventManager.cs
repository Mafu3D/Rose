using System;

namespace Project.Core.GameEvents
{
    public class GameEventManager
    {
        // game event is card/tile/item draw, a choice the player is given, a battle, dialogue, etc. basically anything that interrupts normal flow of the game

        public IGameEvent CurrentGameEvent { get;  private set; }

        public event Action<IGameEvent> OnTileDrawStarted;
        public event Action<IGameEvent> OnTileDrawEnded;

        public event Action<IGameEvent> OnItemDrawStarted;
        public event Action<IGameEvent> OnItemDrawEnded;

        // Tile
        public TileChoiceEvent StartTileDrawEvent(int amount, bool shuffleRemaining)
        {
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running!");

            TileChoiceEvent tileChoiceEvent = new TileChoiceEvent(amount, shuffleRemaining);
            CurrentGameEvent = tileChoiceEvent;
            tileChoiceEvent.SetupEvent();
            OnTileDrawStarted?.Invoke(tileChoiceEvent);
            return tileChoiceEvent;
        }

        public void EndTileDrawEvent()
        {
            IGameEvent gameEvent = CurrentGameEvent;
            CurrentGameEvent = null;
            OnTileDrawEnded?.Invoke(gameEvent);
        }

        // Item
        public ItemChoiceEvent StartItemDrawEvent(int amount, bool shuffleRemaining)
        {
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running!");

            ItemChoiceEvent itemChoiceEvent = new ItemChoiceEvent(amount, shuffleRemaining);
            CurrentGameEvent = itemChoiceEvent;
            itemChoiceEvent.SetupEvent();
            OnItemDrawStarted?.Invoke(itemChoiceEvent);
            return itemChoiceEvent;
        }

        public void EndItemDrawEvent()
        {
            IGameEvent gameEvent = CurrentGameEvent;
            CurrentGameEvent = null;
            OnItemDrawEnded?.Invoke(gameEvent);
        }
    }
}