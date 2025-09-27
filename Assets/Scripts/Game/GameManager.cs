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

        [SerializeField] public DeckData EncounterDeckData;
        [SerializeField] public DeckData MonsterDeckData;
        [SerializeField] public DeckData ItemDeckData;

        [field: SerializeField] public float TimeBetweenPlayerMoves { get; private set; } = 0.25f;

        public GridManager Grid;

        public HeroNode Hero => Player.HeroNode;
        public int Turn { get; private set; }

        public StateMachine stateMachine;

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
            stateMachine = new StateMachine();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            stateMachine.Update();
        }

        public void IncrementTurn()
        {
            Turn += 1;
            Hero.ResetMovesRemaining();
            OnStartPlayerTurn?.Invoke();
        }

        public void StartGame()
        {
            stateMachine.SwitchState(new PlayerMove(new GameRunning(stateMachine), stateMachine));
            Turn = 0;

            EncounterDeck = new Deck();
            EncounterDeck.AddCards(EncounterDeckData.UnpackCards());
            EncounterDeck.Reset();
            EncounterDeck.Shuffle();
        }
    }
}


