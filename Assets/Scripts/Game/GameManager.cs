using System;
using System.Collections.Generic;
using UnityEngine;
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
        [SerializeField] public float TimeBetweenPlayerMoves = 0.25f;

        public GameGrid Grid;

        public HeroNode Hero => Player.HeroNode;
        public int Turn { get; private set; }

        public StateMachine StateMachine;
        public EffectQueue EffectQueue;

        public Deck<Card> EncounterDeck;
        public Deck<Card> MonsterDeck;
        public Deck<Item> ItemDeck;

        public event Action OnStartPlayerTurn;
        public event Action OnEndPlayerTurn;
        public event Action OnEndTurn;

        public Choice<Item> ActiveTreasureChoice;
        public event Action OnTresureChoiceStarted;
        public event Action OnTresureChoiceEnded;
        public bool IsChoosingTreasure => ActiveTreasureChoice != null;


        public Choice<Card> ActiveCardChoice;
        public event Action OnCardChoiceStarted;
        public event Action OnCardChoiceEnded;
        public bool IsChoosingCard => ActiveCardChoice != null;


        private List<Node> nodesToBeDestroyed = new();

        protected override void Awake()
        {
            base.Awake();

            Grid = new GameGrid(new Vector2(1, 1));
            TEMP_BUILD_MAP();

            StateMachine = new StateMachine();
            EffectQueue = new EffectQueue();
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

        // public void EndPlayerTurn()
        // {

        // }

        // private void DrawEncounterCard()
        // {
        //     Card card = EncounterDeck.Draw();
        //     if (card == null)
        //     {
        //         return;
        //     }
        //     if (card.CardType == CardType.Monster)
        //     {
        //         card = MonsterDeck.Draw();
        //         if (card == null)
        //         {
        //             return;
        //         }
        //     }
        //     MainUI.Instance.DisplayCard(card);
        // }

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

        public void StartNewCardChoice(Choice<Card> cardChoice)
        {
            ActiveCardChoice = cardChoice;
            MainUI.Instance.DisplayCards(ActiveCardChoice.GetAllItems());
            // MainUI.Instance.DisplayTreasureChoice(cardChoice);
            OnCardChoiceStarted?.Invoke();
        }

        public void EndCardChoice()
        {
            if (ActiveCardChoice != null)
            {
                ActiveCardChoice = null;
                // MainUI.Instance.DestroyTreasureChoice();
                MainUI.Instance.DestroyDisplayedCards();
                OnCardChoiceEnded?.Invoke();
            }
        }

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
                new int[] {6, 4}
            };

            foreach (int[] coord in coords)
            {
                walkableCells.Add(new Cell(coord[0], coord[1], new Vector2(1, 1)));
            }
            Grid.RegisterWalkableCells(walkableCells);
        }
    }
}


