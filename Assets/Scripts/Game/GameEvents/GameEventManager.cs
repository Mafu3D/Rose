using System;
using System.Collections.Generic;
using System.Linq;
using Project.Decks;
using Project.Items;
using Project.NPCs;
using UnityEngine;

namespace Project.Core.GameEvents
{
    public class GameEventManager
    {
        // game event is card/tile/item draw, a choice the player is given, a battle, dialogue, etc. basically anything that interrupts normal flow of the game
        public TileChoiceEvent CurrentTileChoiceEvent { get; private set; }
        public ItemChoiceEvent CurrentItemChoiceEvent { get; private set; }
        public CardChoiceEvent CurrentCardChoiceEvent { get; private set; }
        public ShopEvent CurrentShopEvent { get; private set; }
        public ServiceEvent CurrentServiceEvent { get; private set; }
        public InventoryChoiceEvent CurrentInventoryChoiceEvent { get; private set; }

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

        public event Action<IGameEvent> OnInventoryChoiceStarted;
        public event Action<IGameEvent> OnInventoryChoiceEnded;

        public bool GameEventActive()
        {
            return CurrentTileChoiceEvent != null || CurrentItemChoiceEvent != null || CurrentCardChoiceEvent != null || CurrentShopEvent != null || CurrentServiceEvent != null || CurrentInventoryChoiceEvent != null;
        }

        // Tile
        public TileChoiceEvent StartTileDrawEvent(int amount, bool shuffleRemaining)
        {
            if (CurrentTileChoiceEvent != null) throw new Exception($"A game event is already running: {CurrentTileChoiceEvent.GetType()}");

            TileChoiceEvent tileChoiceEvent = new TileChoiceEvent(amount, shuffleRemaining);
            CurrentTileChoiceEvent = tileChoiceEvent;
            tileChoiceEvent.SetupEvent();
            OnTileDrawStarted?.Invoke(tileChoiceEvent);
            return tileChoiceEvent;
        }

        public void EndTileDrawEvent()
        {
            TileChoiceEvent gameEvent = CurrentTileChoiceEvent;
            CurrentTileChoiceEvent = null;
            OnTileDrawEnded?.Invoke(gameEvent);
        }

        // Item
        public ItemChoiceEvent StartItemDrawEvent(int amount, bool shuffleRemaining)
        {
            Debug.Log("Starting new ItemChoiceEvent");
            if (CurrentItemChoiceEvent != null) throw new Exception($"A game event is already running: {CurrentItemChoiceEvent.GetType()}");

            ItemChoiceEvent itemChoiceEvent = new ItemChoiceEvent(amount, shuffleRemaining);
            CurrentItemChoiceEvent = itemChoiceEvent;
            itemChoiceEvent.SetupEvent();
            OnItemDrawStarted?.Invoke(itemChoiceEvent);
            return itemChoiceEvent;
        }

        public void EndItemDrawEvent()
        {
            Debug.Log($"Ending event: {CurrentItemChoiceEvent}");
            ItemChoiceEvent gameEvent = CurrentItemChoiceEvent;
            CurrentItemChoiceEvent = null;
            OnItemDrawEnded?.Invoke(gameEvent);
        }

        // Monster Draw
        public CardChoiceEvent StartCardDrawEvent(Deck<Card> deck, int amount, bool shuffleRemaining)
        {
            Debug.Log("Starting new CardDrawEvent");
            if (CurrentCardChoiceEvent != null) throw new Exception($"A game event is already running: {CurrentCardChoiceEvent.GetType()}");

            CardChoiceEvent cardChoiceEvent = new CardChoiceEvent(deck, amount, shuffleRemaining);
            CurrentCardChoiceEvent = cardChoiceEvent;
            cardChoiceEvent.SetupEvent();
            OnCardDrawStarted?.Invoke(cardChoiceEvent);
            return cardChoiceEvent;
        }

        public void EndCardDrawEvent()
        {
            Debug.Log($"Ending event: {CurrentCardChoiceEvent}");
            CardChoiceEvent gameEvent = CurrentCardChoiceEvent;
            CurrentCardChoiceEvent = null;
            OnCardDrawEnded?.Invoke(gameEvent);
        }

        // Shop
        public ShopEvent StartShopEvent(int amount, float priceModifier, bool replaceOnBuy = false, bool refreshable = true, int refreshCost = 0, List<ItemData> existingInventory = null)
        {
            Debug.Log("Starting new shop event");
            if (CurrentShopEvent != null) throw new Exception($"A game event is already running: {CurrentShopEvent.GetType()}");

            ShopEvent shopEvent = new ShopEvent(amount, priceModifier, replaceOnBuy, refreshable, refreshCost, existingInventory);
            CurrentShopEvent = shopEvent;
            shopEvent.SetupEvent();
            OnShopStarted?.Invoke(shopEvent);
            return shopEvent;
        }

        public void EndShopEvent()
        {
            Debug.Log($"Ending event: {CurrentShopEvent}");
            ShopEvent gameEvent = CurrentShopEvent;
            CurrentShopEvent = null;
            OnShopEnded?.Invoke(gameEvent);
        }

        // NPC Service
        public ServiceEvent StartNPCServiceEvent(NPCInteractionDefinition npcServiceDefinition)
        {
            Debug.Log("Starting new npc service event");
            if (CurrentServiceEvent != null) throw new Exception($"A game event is already running: {CurrentServiceEvent.GetType()}");

            ServiceEvent npcServiceEvent = new ServiceEvent(npcServiceDefinition.Services.Count, npcServiceDefinition);
            CurrentServiceEvent = npcServiceEvent;
            npcServiceEvent.SetupEvent();
            OnNPCServiceStarted?.Invoke(npcServiceEvent);
            return npcServiceEvent;
        }

        public void EndNPCServiceEvent()
        {
            Debug.Log($"Ending event: {CurrentServiceEvent}");
            ServiceEvent gameEvent = CurrentServiceEvent;
            CurrentServiceEvent = null;
            OnNPCServiceEnded?.Invoke(gameEvent);
        }

        // Inventory Choice
        public InventoryChoiceEvent StartInventoryChoiceEvent(int amount)
        {
            Debug.Log("Starting new npc service event");
            if (CurrentInventoryChoiceEvent != null) throw new Exception($"A game event is already running: {CurrentInventoryChoiceEvent.GetType()}");

            InventoryChoiceEvent inventoryChoiceEvent = new InventoryChoiceEvent(amount);
            CurrentInventoryChoiceEvent = inventoryChoiceEvent;
            inventoryChoiceEvent.SetupEvent();
            OnInventoryChoiceStarted?.Invoke(inventoryChoiceEvent);
            return inventoryChoiceEvent;
        }

        public void EndInventoryChoiceEvent()
        {
            Debug.Log($"Ending event: {CurrentInventoryChoiceEvent}");
            InventoryChoiceEvent gameEvent = CurrentInventoryChoiceEvent;
            CurrentInventoryChoiceEvent = null;
            OnInventoryChoiceEnded?.Invoke(gameEvent);
        }
    }
}