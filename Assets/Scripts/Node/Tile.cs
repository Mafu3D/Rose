using System;
using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
using Project.Items;
using UnityEngine;

namespace Project.GameTiles
{
    public enum TileType
    {
        None,
        Hero,
        Monster,
        Danger,
        Economic,
        Event,
        NPC,
        Boon,
        Special
    }

    public class Tile : MonoBehaviour
    {
        public Cell CurrentCell;
        private SpriteRenderer mySpriteRenderer;
        [SerializeField] public TileData TileData;
        [SerializeField] public CharacterData CharacterData;
        [SerializeField] protected int moveDistance = 1;
        [SerializeField] private bool willMoveTowardsPlayer = false;
        [SerializeField] public bool IsPlayer = true;

        [Header("Icons")]
        [SerializeField] SpriteRenderer icon;
        [SerializeField] SpriteRenderer outline1;
        [SerializeField] SpriteRenderer outline2;
        [SerializeField] GameObject usableIcon;
        [SerializeField] GameObject actionIcon;

        private bool isStunned = true;
        private int roundsStunned = 1;
        public Character Character { get; private set; }

        protected Rigidbody2D myRigidBody;

        // TEMP: REMOVE
        [Header("Activations")]
        public int ActivatesThisGame;
        public int ActivatesThisTurn;
        public int PlayerEntersThisGame;
        public int PlayerEntersThisTurn;
        public int PlayerExitsThisGame;
        public int PlayerExitsThisTurn;


        public int MovesRemaining { get; private set; }
        public Action OnRemainingMovesChanged;

        void OnValidate()
        {
            SetIconAndColor();
        }

        public void SetTileData(TileData tileData)
        {
            TileData = tileData;
            SetIconAndColor();
        }

        private void SetIconAndColor()
        {
            if (TileData != null)
            {
                if (icon != null)
                {
                    icon.sprite = TileData.Sprite;
                }

                string hexColor = "#222323";
                if (outline1 != null)
                {
                    switch (TileData.TileType)
                    {
                        case TileType.Hero:
                        case TileType.Monster:
                            hexColor = "#222323";
                            break;
                        case TileType.NPC:
                            hexColor = "#5EA4DB";
                            break;
                        case TileType.Danger:
                            hexColor = "#B24242";
                            break;
                        case TileType.Economic:
                            hexColor = "#CAB742";
                            break;
                        case TileType.Event:
                            hexColor = "#F18A31";
                            break;
                        case TileType.Boon:
                            hexColor = "#649061";
                            break;
                        case TileType.Special:
                            hexColor = "#C555A0";
                            break;

                    }
                    Color color;
                    if (ColorUtility.TryParseHtmlString(hexColor, out color))
                    {
                        outline1.color = color;
                    }
                }

                if (outline2 != null)
                {
                    string hexColor2 = hexColor;
                    if (TileData.SecondaryTileType != TileType.None)
                    {
                        switch (TileData.SecondaryTileType)
                        {
                            case TileType.Hero:
                            case TileType.Monster:
                                hexColor2 = "#222323";
                                break;
                            case TileType.NPC:
                                hexColor2 = "#5EA4DB";
                                break;
                            case TileType.Danger:
                                hexColor2 = "#B24242";
                                break;
                            case TileType.Economic:
                                hexColor2 = "#CAB742";
                                break;
                            case TileType.Event:
                                hexColor2 = "#F18A31";
                                break;
                            case TileType.Boon:
                                hexColor2 = "#649061";
                                break;
                            case TileType.Special:
                                hexColor2 = "#C555A0";
                                break;

                        }
                    }
                    Color color;
                    if (ColorUtility.TryParseHtmlString(hexColor2, out color))
                    {
                        outline2.color = color;
                    }
                }
            }
        }

        protected virtual void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            if (mySpriteRenderer != null)
            {
                mySpriteRenderer.sprite = TileData.Sprite;
            }

            myRigidBody = GetComponent<Rigidbody2D>();

            if (CharacterData != null)
            {
                Inventory inventory = GetComponent<Inventory>();
                Character = new Character(CharacterData, inventory);
            }
        }

        protected virtual void Start()
        {
            GameManager.Instance.OnGameStartEvent += Initialize;
            GameManager.Instance.OnPlayerMoveEvent += MoveTowardsPlayer;

            if (usableIcon != null) usableIcon.SetActive(true);
            if (actionIcon != null) actionIcon.SetActive(false);
        }

        void Update()
        {
            if (!IsPlayer)
            {
                if (!CanActivate() && !CanPlayerEnter() && !CanPlayerExit())
                {
                    Color color = icon.color;
                    color.a = 0.5f;
                    icon.color = color;
                    usableIcon.SetActive(false);
                }
                else
                {
                    Color color = icon.color;
                    color.a = 1.0f;
                    icon.color = color;
                    usableIcon.SetActive(true);
                }
            }
        }

        private void Initialize()
        {
            RegisterToGrid();
            ResetMovesRemaining();
        }

        public void ResetForTurn()
        {
            ActivatesThisTurn = 0;
            PlayerEntersThisTurn = 0;
            PlayerExitsThisTurn = 0;
        }

        public void ResetForGame()
        {
            ActivatesThisGame = 0;
            PlayerEntersThisGame = 0;
            PlayerExitsThisGame = 0;
        }

        public bool CanActivate()
        {
            if (TileData.OnActivateStrategies.Count > 0)
            {
                if (ActivatesThisTurn == 0 ||
                    (TileData.CanBeUsedMultipleTimes && TileData.UsesPerTurn > ActivatesThisTurn))
                    {
                        if ((TileData.LimitTotalUses && ActivatesThisGame < TileData.TotalUses) || !TileData.LimitTotalUses)
                        {
                            return true;
                        }
                    }
            }
            return false;
        }

        public bool CanPlayerEnter()
        {
            if (TileData.OnPlayerEnterStrategies.Count > 0)
            {
                if (PlayerEntersThisTurn == 0 ||
                    (TileData.CanBeUsedMultipleTimes && TileData.UsesPerTurn > PlayerEntersThisTurn))
                    {
                        if ((TileData.LimitTotalUses && PlayerEntersThisGame < TileData.TotalUses) || !TileData.LimitTotalUses)
                        {
                            return true;
                        }
                    }
            }
            return false;
        }

        public bool CanPlayerExit()
        {
            if (TileData.OnPlayerExitStrategies.Count > 0)
            {
                if (PlayerExitsThisTurn == 0 ||
                    (TileData.CanBeUsedMultipleTimes && TileData.UsesPerTurn > PlayerExitsThisTurn))
                    {
                        if ((TileData.LimitTotalUses && PlayerExitsThisGame < TileData.TotalUses) || !TileData.LimitTotalUses)
                        {
                            return true;
                        }
                    }
            }
            return false;
        }

        public void RegisterCharacterFromData(CharacterData characterData)
        {
            if (Character != null)
            {
                Inventory inventory = GetComponent<Inventory>();
                Character = new Character(characterData, inventory);
            }
        }

        public void RegisterCharacter(Character character)
        {
            if (Character != null)
            {
                Inventory inventory = GetComponent<Inventory>();
                Character = character;
                ResetMovesRemaining(); // do this because reset moves takes infro
            }
        }

        public void ModifyMovesRemaining(int amount)
        {
            MovesRemaining += amount;
            OnRemainingMovesChanged?.Invoke();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
            Debug.Log($"Registering {this} to Cell {CurrentCell.ToString()}");
        }

        public virtual void Move(Vector2 direction)
        {
            if (direction != Vector2.zero && MovesRemaining > 0)
            {
                GameManager.Instance.Grid.DeregisterFromCell(CurrentCell, this);
                Cell destinationCell = GameManager.Instance.Grid.GetNeighborCell(CurrentCell, direction * moveDistance);
                if (GameManager.Instance.Grid.TryGetCellInWalkableCells(destinationCell))
                {
                    CurrentCell = destinationCell;
                    myRigidBody.MovePosition(destinationCell.Center);
                    GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);

                    MovesRemaining -= 1;
                    OnRemainingMovesChanged?.Invoke();
                }
            }
        }

        public virtual void MoveToCell(Cell cell)
        {
            if (cell != CurrentCell && MovesRemaining > 0)
            {
                GameManager.Instance.Grid.DeregisterFromCell(CurrentCell, this);
                if (GameManager.Instance.Grid.TryGetCellInWalkableCells(cell))
                {
                    CurrentCell = cell;
                    myRigidBody.MovePosition(CurrentCell.Center);
                    GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);

                    MovesRemaining -= 1;
                    OnRemainingMovesChanged?.Invoke();
                }
            }
        }

        public void ResetMovesRemaining()
        {
            if (Character != null)
            {
                MovesRemaining = Math.Clamp(Character.Attributes.GetAttributeValue(AttributeType.Speed), 1, 99);
                OnRemainingMovesChanged?.Invoke();
            }
        }

        private void MoveTowardsPlayer()
        {
            // This is all so gross
            if (isStunned)
            {
                roundsStunned -= 1;
                if (roundsStunned <= 0)
                {
                    isStunned = false;
                }
                return;
            }

            if (willMoveTowardsPlayer)
            {
                List<Cell> path = GameManager.Instance.Grid.GetPathBetweenTwoCells(this.CurrentCell, GameManager.Instance.Player.HeroTile.CurrentCell, true);
                if (path.Count > 0)
                {
                    MoveToCell(path[0]);
                    if (CurrentCell == GameManager.Instance.Player.HeroTile.CurrentCell)
                    {
                        isStunned = true;
                        roundsStunned = 1;
                    }
                }
            }
        }
    }
}
