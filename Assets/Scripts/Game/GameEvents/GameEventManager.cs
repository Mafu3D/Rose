using System;
using System.Linq;
using Project.Decks;
using Project.NPCs;
using UnityEngine;

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

        public event Action<IGameEvent> OnCardDrawStarted;
        public event Action<IGameEvent> OnCardDrawEnded;

        public event Action<IGameEvent> OnShopStarted;
        public event Action<IGameEvent> OnShopEnded;

        public event Action<IGameEvent> OnNPCServiceStarted;
        public event Action<IGameEvent> OnNPCServiceEnded;

        // Tile
        public TileChoiceEvent StartTileDrawEvent(int amount, bool shuffleRemaining)
        {
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running: {CurrentGameEvent.GetType()}");

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
            Debug.Log("Starting new ItemChoiceEvent");
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running: {CurrentGameEvent.GetType()}");

            ItemChoiceEvent itemChoiceEvent = new ItemChoiceEvent(amount, shuffleRemaining);
            CurrentGameEvent = itemChoiceEvent;
            itemChoiceEvent.SetupEvent();
            OnItemDrawStarted?.Invoke(itemChoiceEvent);
            return itemChoiceEvent;
        }

        public void EndItemDrawEvent()
        {
            Debug.Log($"Ending event: {CurrentGameEvent}");
            IGameEvent gameEvent = CurrentGameEvent;
            CurrentGameEvent = null;
            OnItemDrawEnded?.Invoke(gameEvent);
        }

        // Monster Draw
        public CardChoiceEvent StartCardDrawEvent(Deck<Card> deck, int amount, bool shuffleRemaining)
        {
            Debug.Log("Starting new CardDrawEvent");
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running: {CurrentGameEvent.GetType()}");

            CardChoiceEvent cardChoiceEvent = new CardChoiceEvent(deck, amount, shuffleRemaining);
            CurrentGameEvent = cardChoiceEvent;
            cardChoiceEvent.SetupEvent();
            OnCardDrawStarted?.Invoke(cardChoiceEvent);
            return cardChoiceEvent;
        }

        public void EndCardDrawEvent()
        {
            Debug.Log($"Ending event: {CurrentGameEvent}");
            IGameEvent gameEvent = CurrentGameEvent;
            CurrentGameEvent = null;
            OnCardDrawEnded?.Invoke(gameEvent);
        }

        // Shop
        public ShopEvent StartShopEvent(int amount, float priceModifier, bool replaceOnBuy = false, bool refreshable = true, int refreshCost = 0)
        {
            Debug.Log("Starting new shop event");
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running: {CurrentGameEvent.GetType()}");

            ShopEvent shopEvent = new ShopEvent(amount, priceModifier, replaceOnBuy, refreshable, refreshCost);
            CurrentGameEvent = shopEvent;
            shopEvent.SetupEvent();
            OnShopStarted?.Invoke(shopEvent);
            return shopEvent;
        }

        public void EndShopEvent()
        {
            Debug.Log($"Ending event: {CurrentGameEvent}");
            IGameEvent gameEvent = CurrentGameEvent;
            CurrentGameEvent = null;
            OnShopEnded?.Invoke(gameEvent);
        }

        // NPC Service
        public ServiceEvent StartNPCServiceEvent(NPCInteractionDefinition npcServiceDefinition)
        {
            Debug.Log("Starting new npc service event");
            if (CurrentGameEvent != null) throw new Exception($"A game event is already running: {CurrentGameEvent.GetType()}");

            ServiceEvent npcServiceEvent = new ServiceEvent(npcServiceDefinition.Services.Count, npcServiceDefinition);
            CurrentGameEvent = npcServiceEvent;
            npcServiceEvent.SetupEvent();
            OnNPCServiceStarted?.Invoke(npcServiceEvent);
            return npcServiceEvent;
        }

        public void EndNPCServiceEvent()
        {
            Debug.Log($"Ending event: {CurrentGameEvent}");
            IGameEvent gameEvent = CurrentGameEvent;
            CurrentGameEvent = null;
            OnNPCServiceEnded?.Invoke(gameEvent);
        }
    }
}