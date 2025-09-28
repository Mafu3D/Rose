using System;
using System.Collections.Generic;
using UnityEngine;
using Project.Grid;
using Project.GameNode.Hero;
using Project.GameNode;
using Project.States;
using Project.PlayerSystem;
using Project.GameStates;
using Project.Decks;
using Project.Items;
using Project.UI.MainUI;

namespace Project
{

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public Player Player;
        [SerializeField] public GameObject UICanvas;

        [SerializeField] public DeckData EncounterDeckData;
        [SerializeField] public DeckData MonsterDeckData;
        [SerializeField] public ItemDeckData ItemDeckData;

        [field: SerializeField] public float TimeBetweenPlayerMoves { get; private set; } = 0.25f;

        public GridManager Grid;

        public HeroNode Hero => Player.HeroNode;
        public int Turn { get; private set; }

        public StateMachine StateMachine;

        public Deck<Card> EncounterDeck;
        public Deck<Card> MonsterDeck;
        public Deck<Item> ItemDeck;

        public event Action OnStartPlayerTurn;
        public event Action OnEndPlayerTurn;
        public event Action OnEndTurn;

        public TreasureChoice ActiveTreasureChoice;
        public event Action OnTresureChoiceStarted;
        public event Action OnTresureChoiceEnded;
        public bool IsChoosingTreasure => ActiveTreasureChoice != null;

        protected override void Awake()
        {
            base.Awake();
            Grid = new GridManager(new Vector2(1, 1));
            StateMachine = new StateMachine();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            StateMachine.Update();
        }

        public void IncrementTurn()
        {
            Turn += 1;
            Hero.ResetMovesRemaining();
            OnStartPlayerTurn?.Invoke();
        }

        public void StartGame()
        {
            StateMachine.SwitchState(new PlayerMove(new PlayerTurn(StateMachine), StateMachine));
            Turn = 0;

            EncounterDeck = InitializeCardDeck(EncounterDeckData);
            MonsterDeck = InitializeCardDeck(MonsterDeckData);
            ItemDeck = InitializeItemDeck(ItemDeckData);
        }

        private Deck<Card> InitializeCardDeck(DeckData deckData)
        {
            Deck<Card> deck = new Deck<Card>();
            deck.AddPermanent(deckData.UnpackCards());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }

        private Deck<Item> InitializeItemDeck(ItemDeckData deckData)
        {
            Deck<Item> deck = new Deck<Item>();
            deck.AddPermanent(deckData.UnpackItems());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }

        public void StartNewTreasureChoice(TreasureChoice treasureChoice)
        {
            ActiveTreasureChoice = treasureChoice;
            MainUI.Instance.DisplayTreasureChoice(treasureChoice);
            OnTresureChoiceStarted?.Invoke();
        }

        public void EndTreasureChoice()
        {
            if (ActiveTreasureChoice != null)
            {
                ActiveTreasureChoice = null;
                MainUI.Instance.DestroyTreasureChoice();
                OnTresureChoiceEnded?.Invoke();
            }
        }
    }
}


