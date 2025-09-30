using System;
using System.Collections.Generic;
using UnityEngine;
using Project.GameNode.Hero;
using Project.GameNode;
using Project.PlayerSystem;
using Project.Decks;
using Project.Items;
using Project.UI.MainUI;
using Project.GameLoop;
using UnityEngine.Events;
using Project.Core;

namespace Project
{

    public class GameManager : Singleton<GameManager>
    {
        [Header("References I need to move out later")]
        [SerializeField] public Player Player;
        [SerializeField] public GameObject UICanvas;

        [Header("Game Parameters")]
        [SerializeField] public DeckData EncounterDeckData;
        [SerializeField] public DeckData MonsterDeckData;
        [SerializeField] public ItemDeckData ItemDeckData;

        [Header("Game Settings")]
        [SerializeField] public bool AutoBattle = true;
        [SerializeField] public float AutoBattleSpeed = 1f;
        [SerializeField] public float MinTimeBetweenPlayerMoves = 0.25f;
        [SerializeField] public float MinTimeBetweenPhases = 0.5f;

        public GameGrid Grid;

        public HeroNode Hero => Player.HeroNode;
        public int Round { get; private set; }

        public StateMachine StateMachine;
        public EffectQueue EffectQueue;

        public CardDrawManager CardDrawManager;
        public Deck<Card> EncounterDeck;
        public Deck<Card> MonsterDeck;
        public Deck<Item> ItemDeck;

        public event Action OnGameStartEvent;
        public event Action OnRoundStartEvent;
        public event Action OnTurnStartEvent;
        public event Action OnPlayerMoveStartEvent;
        public event Action OnPlayerMoveEvent;
        public event Action OnEndOfTurnEvent;
        public event Action OnActivateTilesEvent;
        public event Action OnDrawCardEvent;
        public event Action OnEndOfRoundEvent;

        public Choice<Item> ActiveTreasureChoice;
        public event Action OnTresureChoiceStarted;
        public event Action OnTresureChoiceEnded;
        public bool IsChoosingTreasure => ActiveTreasureChoice != null;


        public Choice<Card> ActiveCardChoice;
        public event Action OnCardChoiceStarted;
        public event Action OnCardChoiceEnded;
        public bool IsChoosingCard => ActiveCardChoice != null;


        private List<Node> nodesToBeDestroyed = new();

        public bool GameHasStarted { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            EffectQueue.Update();
            StateMachine.Update();
        }

        #region Start New Game

        public void StartGame()
        {
            Grid = new GameGrid(new Vector2(1, 1));
            TEMP_BUILD_MAP();

            EncounterDeck = InitializeCardDeck(EncounterDeckData);
            MonsterDeck = InitializeCardDeck(MonsterDeckData);
            ItemDeck = InitializeItemDeck(ItemDeckData);

            CardDrawManager = new CardDrawManager();

            EffectQueue = new EffectQueue();

            // State Machine
            StateMachine = new StateMachine();
            StateMachine.SetInitialState(new RoundStartState("Initial Round Start", StateMachine, this)); // Move this down to have round start effects trigger on game start

            // // Declare States
            // var roundStartState = new RoundStartState("RoundStart", this);
            // var roundStartResolveState = new ResolveEffectsState("RoundStartResolve", this);

            // var turnStartState = new TurnStartState("TurnStart", this);
            // var turnStartResolveState = new ResolveEffectsState("TurnStartResolve", this);

            // var playerMoveState = new PlayerMoveState("PlayerMove", this);
            // var playerMoveResolveState = new ResolveEffectsState("PlayerMoveResolve", this);
            // var playerMoveEndResolveState = new ResolveEffectsState("PlayerMoveEndResolve", this);

            // var endOfTurnState = new EndOfTurnState("EndOfTurn", this);
            // var endOfTurnResolveState = new ResolveEffectsState("EndOfTurnResolve", this);

            // // Debug.Log(roundStartResolveState);
            // // Debug.Log(endOfTurnResolveState);
            // // Debug.Log(roundStartResolveState == endOfTurnResolveState);
            // // Debug.Log(Object.ReferenceEquals(roundStartResolveState, endOfTurnResolveState));

            // // Define Transitions
            // At(roundStartState, roundStartResolveState, new FuncPredicate(() => PhaseSwitch.PhaseIsComplete));
            // At(roundStartResolveState, turnStartState, new FuncPredicate(() => !EffectQueue.QueueNeedsToBeResolved));

            // At(turnStartState, turnStartResolveState, new FuncPredicate(() => PhaseSwitch.PhaseIsComplete));
            // At(turnStartResolveState, playerMoveState, new FuncPredicate(() => !EffectQueue.QueueNeedsToBeResolved));

            // At(playerMoveState, playerMoveResolveState, new FuncPredicate(() => PhaseSwitch.PhaseIsComplete));
            // At(playerMoveResolveState, playerMoveState, new FuncPredicate(() => !EffectQueue.QueueNeedsToBeResolved));

            // At(playerMoveState, playerMoveEndResolveState, new FuncPredicate(() => PhaseSwitch.PhaseIsComplete));
            // At(playerMoveState, playerMoveEndResolveState, new FuncPredicate(() => Hero.MovesRemaining == 0));
            // At(playerMoveEndResolveState, endOfTurnState, new FuncPredicate(() => !EffectQueue.QueueNeedsToBeResolved));

            // At(endOfTurnState, endOfTurnResolveState, new ActionPredicate(OnRoundStartEvent));
            // At(endOfTurnResolveState, roundStartState, new FuncPredicate(() => !EffectQueue.QueueNeedsToBeResolved));

            // // Set initial state
            // StateMachine.SetState(roundStartState);

            OnGameStartEvent?.Invoke();
            Round = 0;
            OnNewRound();
        }

        // void At(IState from, IState to, IPredicate condition) => StateMachine.AddTransition(from, to, condition);
        // void Any(IState to, IPredicate condition) => StateMachine.AddAnyTransition(to, condition);

        #endregion

        #region Game Loop
        // Everthing about the game stat itself should be updated here! (What turn is it, shuffling decks, etc.)

        public void OnNewRound()
        {
            Round += 1;
            Hero.ResetMovesRemaining();
            OnRoundStartEvent?.Invoke();
        }

        public void OnNewTurn()
        {
            OnTurnStartEvent?.Invoke();
        }

        public void OnStartPlayerMove()
        {
            OnPlayerMoveStartEvent?.Invoke();
        }

        public void OnPlayerMove()
        {
            OnPlayerMoveEvent?.Invoke();
        }

        public void OnEndOfTurn()
        {
            OnEndOfTurnEvent?.Invoke();
        }

        public void OnActivateTiles()
        {
            OnActivateTilesEvent?.Invoke();
        }

        public void OnDrawCard()
        {
            OnDrawCardEvent?.Invoke();
        }

        public void OnEndOfRound()
        {
            OnEndOfRoundEvent?.Invoke();
        }

        #endregion









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

        public void StartNewTreasureChoice(Choice<Item> treasureChoice)
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

        // public void StartNewCardChoice(Choice<Card> cardChoice)
        // {
        //     ActiveCardChoice = cardChoice;
        //     MainUI.Instance.DisplayCards(ActiveCardChoice.GetAllItems());
        //     // MainUI.Instance.DisplayTreasureChoice(cardChoice);
        //     OnCardChoiceStarted?.Invoke();
        // }

        // public void EndCardChoice()
        // {
        //     if (ActiveCardChoice != null)
        //     {
        //         ActiveCardChoice = null;
        //         // MainUI.Instance.DestroyTreasureChoice();
        //         MainUI.Instance.DestroyDisplayedCards();
        //         OnCardChoiceEnded?.Invoke();
        //     }
        // }

        public void MarkNodeForDestroy(Node node)
        {
            if (node == null) return;

            if (!nodesToBeDestroyed.Contains(node))
            {
                node.gameObject.SetActive(false);
                nodesToBeDestroyed.Add(node);
            }
        }

        public void DestroyMarkedNodes()
        {
            foreach (Node node in nodesToBeDestroyed)
            {
                Destroy(node.gameObject);
            }
            nodesToBeDestroyed = new();
        }



        #region Temp Shit
        private void TEMP_BUILD_MAP()
        {
            List<Cell> walkableCells = new List<Cell>();
            List<int[]> coords = new List<int[]> {
                new int[] {-5, 5},
                new int[] {-4, 5},
                new int[] {-3, 5},
                new int[] {-2, 5},
                new int[] {-1, 5},
                new int[] {1, 5},
                new int[] {2, 5},
                new int[] {3, 5},
                new int[] {4, 5},
                new int[] {5, 5},
                new int[] {5, 4},
                new int[] {5, 3},
                new int[] {5, 2},
                new int[] {6, 2},
                new int[] {7, 2},
                new int[] {7, 1},
                new int[] {8, 1},
                new int[] {8, -1},
                new int[] {8, -2},
                new int[] {8, -3},
                new int[] {7, -3},
                new int[] {7, -4},
                new int[] {6, -2},
                new int[] {6, -3},
                new int[] {6, -4},
                new int[] {5, -2},
                new int[] {5, -3},
                new int[] {5, -4},
                new int[] {5, -1},
                new int[] {4, -1},
                new int[] {3, -1},
                new int[] {2, -1},
                new int[] {1, -1},
                new int[] {1, -2},
                new int[] {1, -3},
                new int[] {1, -4},
                new int[] {-1, -4},
                new int[] {-2, -4},
                new int[] {-3, -4},
                new int[] {-4, -4},
                new int[] {-4, -3},
                new int[] {-5, -3},
                new int[] {-5, -2},
                new int[] {-6, -3},
                new int[] {-7, -3},
                new int[] {-7, -2},
                new int[] {-7, -1},
                new int[] {-7, 1},
                new int[] {-7, 2},
                new int[] {-7, 3},
                new int[] {-7, 4},
                new int[] {-6, 4},
                new int[] {-5, 4},
                new int[] {-3, -3},
                new int[] {-3, -2},
                new int[] {-3, -1},
                new int[] {-3, 1},
                new int[] {-4, 1},
                new int[] {-2, 1},
                new int[] {-2, 2},
                new int[] {-1, 2},
                new int[] {-1, 3},
                new int[] {-1, 4},
                new int[] {6, 4},
                // new int[] {7, 4},
                // new int[] {8, 4},
                // new int[] {8, 3},
                // new int[] {8, 2},
                // new int[] {9, 2},
                // new int[] {10, 2},
            };

            foreach (int[] coord in coords)
            {
                walkableCells.Add(new Cell(coord[0], coord[1], new Vector2(1, 1)));
            }
            Grid.RegisterWalkableCells(walkableCells);
        }
        #endregion
    }
}