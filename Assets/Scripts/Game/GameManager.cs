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

namespace Project
{

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] public Player Player;
        [SerializeField] public GameObject UICanvas;

        [SerializeField] public DeckData EncounterDeckData;
        [SerializeField] public DeckData MonsterDeckData;
        [SerializeField] public DeckData ItemDeckData;

        [field: SerializeField] public float TimeBetweenPlayerMoves { get; private set; } = 0.25f;

        public GridManager Grid;

        public HeroNode Hero => Player.HeroNode;
        public int Turn { get; private set; }

        public StateMachine StateMachine;

        public Deck EncounterDeck;
        public Deck MonsterDeck;
        public Deck ItemDeck;

        public event Action OnStartPlayerTurn;
        public event Action OnEndPlayerTurn;
        public event Action OnEndTurn;

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

            EncounterDeck = InitializeDeck(EncounterDeckData);
            MonsterDeck = InitializeDeck(MonsterDeckData);
        }

        private Deck InitializeDeck(DeckData deckData)
        {
            Deck deck = new Deck();
            deck.AddCards(deckData.UnpackCards());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }
    }
}


