using System;
using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
using Project.Items;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.GameTiles
{
    public enum TileType
    {
        Location,
        NPC,
        Hero,
        Combatant,
        Event
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
        private bool isStunned = true;
        private int roundsStunned = 1;
        public Character Character { get; private set; }

        protected Rigidbody2D myRigidBody;


        List<Tile> usedCharacters = new();

        public int MovesRemaining { get; private set; }
        public Action OnRemainingMovesChanged;

        void OnValidate()
        {
            if (TileData != null)
            {
                mySpriteRenderer = GetComponent<SpriteRenderer>();
                mySpriteRenderer.sprite = TileData.Sprite;
            }
        }

        protected virtual void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.sprite = TileData.Sprite;

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
        }

        private void Initialize()
        {
            RegisterToGrid();
            ResetNode();
            ResetMovesRemaining();
        }

        private void ResetNode()
        {
            usedCharacters = new();
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
