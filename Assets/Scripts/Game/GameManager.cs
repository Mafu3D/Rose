using System;
using System.Collections.Generic;
using UnityEngine;
using Project.GameTiles;
using Project.PlayerSystem;
using Project.Decks;
using Project.Items;
using Project.UI.MainUI;
using Project.GameLoop;
using UnityEngine.Events;
using Project.Core;
using Project.Combat;
using Project.States;
using Project.Resources;
using Project.Core.GameEvents;

namespace Project
{

    public class GameManager : Singleton<GameManager>
    {
        [Header("References I need to move out later")]
        [SerializeField] public Player Player;
        [SerializeField] public CharacterData CharacterData;
        [SerializeField] public GameObject UICanvas;

        [Header("Game Parameters")]
        [Range(0, 100)]
        [SerializeField] public float ChanceForMonsterEncounter = 50f;
        [SerializeField] public DeckData EncounterDeckData;
        [SerializeField] public DeckData MonsterDeckData;
        [SerializeField] public DeckData EliteMonsterDeckData;
        [SerializeField] public ItemDeckData ItemDeckData;
        [SerializeField] public TileDeckData TileDeckData;
        [SerializeField] public int RoundsTillBoss;
        [SerializeField] public CharacterDeckData BossDeckData;

        [Header("Game Settings")]
        [SerializeField] public bool AutoBattle = true;
        [SerializeField] public float AutoBattleSpeed = 1f;
        [SerializeField] public float MinTimeBetweenPlayerMoves = 0.25f;
        [SerializeField] public float MinTimeBetweenPhases = 0.5f;

        [Header("Debug")]
        [SerializeField] public bool BattleDebugMode = false;


        // please remove
        public bool WillActivateTilesThisTurn = true;


        public GameGrid Grid;

        public Tile Hero => Player.HeroTile;
        public int Round { get; private set; }

        public StateMachine StateMachine;
        public EffectQueue EffectQueue;

        public BattleManager BattleManager;
        public CardDrawManager CardDrawManager;
        public TileDrawManager TileDrawManager;
        public GameEventManager GameEventManager;
        public Deck<Card> EncounterDeck;
        public Deck<Card> MonsterDeck;
        public Deck<Card> EliteMonsterDeck;
        public Deck<ItemData> ItemDeck;
        public Deck<TileData> TileDeck;
        public Deck<CharacterData> BossDeck;

        public CharacterData Boss;

        public event Action OnGameStartEvent;
        public event Action OnRoundStartEvent;
        public event Action OnTurnStartEvent;
        public event Action OnPlayerMoveStartEvent;
        public event Action OnPlayerMoveEvent;
        public event Action OnEndOfTurnEvent;
        public event Action OnActivateTilesEvent;
        public event Action OnDrawCardEvent;
        public event Action OnEndOfRoundEvent;

        public event Action OnTresureChoiceStarted;
        public event Action OnTresureChoiceEnded;


        public Choice<Card> ActiveCardChoice;
        public event Action OnCardChoiceStarted;
        public event Action OnCardChoiceEnded;
        public bool IsChoosingCard => ActiveCardChoice != null;


        private List<Tile> tilesToBeDestroyed = new();

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
            BattleManager.Update();
        }

        #region Start New Game

        public void StartGame()
        {
            // Player.HeroTile.RegisterCharacterFromData(CharacterData);

            Grid = new GameGrid(new Vector2(1, 1));
            TEMP_BUILD_MAP();

            EncounterDeck = InitializeCardDeck(EncounterDeckData);
            MonsterDeck = InitializeCardDeck(MonsterDeckData);
            EliteMonsterDeck = InitializeCardDeck(EliteMonsterDeckData);
            ItemDeck = InitializeItemDeck(ItemDeckData);
            TileDeck = InitializeTileDeck(TileDeckData);
            BossDeck = InitializeBossDeck(BossDeckData);

            Boss = BossDeck.Draw();
            Debug.Log($"The boss for this round is {Boss.DisplayName}");

            CardDrawManager = new CardDrawManager();
            TileDrawManager = new TileDrawManager();
            BattleManager = new BattleManager();
            GameEventManager = new GameEventManager();

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

            int roundsRemaining = RoundsTillBoss - Round;
            if (roundsRemaining == 10)
            {
                CalloutUI.Instance.QueueCallout("Boss arrives in 10 turns", Color.white, 2f);
            }
            else if (roundsRemaining == 5)
            {
                string hexColor;
                hexColor = "#FF4C00";
                Color color;
                ColorUtility.TryParseHtmlString(hexColor, out color);
                CalloutUI.Instance.QueueCallout("Boss arrives in 5 turns", color, 2f);
            }
            else if (roundsRemaining <= 3 && roundsRemaining > 1)
            {
                CalloutUI.Instance.QueueCallout($"Boss arrives in {roundsRemaining} turns", Color.red, 2f);
            }
            else if (roundsRemaining == 1)
            {
                CalloutUI.Instance.QueueCallout($"Last turn", Color.red, 2f);
            }
            else if (roundsRemaining == 0)
            {
                SummonBoss();
            }
            Debug.Log($"The boss will appear in {RoundsTillBoss - Round} rounds!");

            Hero.ResetMovesRemaining();
            OnRoundStartEvent?.Invoke();

            WillActivateTilesThisTurn = true;
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

        private void SummonBoss()
        {
            Debug.Log("The boss is here!");
            // TileFactory.Instance.CreateTile()
            // GameObject gameObject = Instantiate(enemyNodePrefab, GameManager.Instance.Hero.CurrentCell.Center, Quaternion.identity);
            // Tile tile = gameObject.GetComponent<Tile>();
            // tile.RegisterToGrid();
            // tile.RegisterCharacter(right);

            Character left = GameManager.Instance.Hero.Character;
            Character right = new Character(Boss);
            Inventory inventory;
            if (Boss.InventoryDefinition != null)
            {
                inventory = new Inventory(right, Boss.InventoryDefinition);
            }
            else
            {
                inventory = new Inventory(right);
            }
            right.SetInventory(inventory);
            BattleManager.StartNewBattle(left, right, BossBattleConclusion);
        }

        private void BossBattleConclusion(BattleReport battleReport, Character left, Character right)
        {
            switch (battleReport.Resolution)
            {
                case Combat.Resolution.RanAway:
                case Combat.Resolution.Stole:
                    Debug.Log("OH shit, you shouldn't be able to run from the boss! can you please go back and fight them??");
                    break;
                case Combat.Resolution.Victory:
                    Debug.Log("CONGRATS YOU DEFEATED THE BOSS!");
                    break;
                case Combat.Resolution.Defeat:
                    Debug.Log("YOU LOST! BETTER LUCK NEXT TIME");
                    break;
            }
        }




        private Deck<Card> InitializeCardDeck(DeckData deckData)
        {
            Deck<Card> deck = new Deck<Card>();
            deck.AddPermanent(deckData.UnpackCards());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }

        private Deck<ItemData> InitializeItemDeck(ItemDeckData deckData)
        {
            Deck<ItemData> deck = new Deck<ItemData>();
            deck.AddPermanent(deckData.UnpackItems());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }

        private Deck<TileData> InitializeTileDeck(TileDeckData deckData)
        {
            Deck<TileData> deck = new Deck<TileData>();
            deck.AddPermanent(deckData.UnpackItems());
            deck.Reset();
            deck.Shuffle();
            return deck;
        }


        private Deck<CharacterData> InitializeBossDeck(CharacterDeckData bossDeckData)
        {
            Deck<CharacterData> deck = new Deck<CharacterData>();
            deck.AddPermanent(bossDeckData.Unpack());
            deck.Reset();
            deck.Shuffle();
            return deck;
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

        public void MarkTileForDestroy(Tile tile)
        {
            if (tile == null) return;

            if (!tilesToBeDestroyed.Contains(tile))
            {
                tile.gameObject.SetActive(false);
                tilesToBeDestroyed.Add(tile);
            }
        }

        public void DestroyMarkedTiles()
        {
            foreach (Tile tilee in tilesToBeDestroyed)
            {
                Destroy(tilee.gameObject);
            }
            tilesToBeDestroyed = new();
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
                new int[] {-6, -4},
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