using System;
using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.GameNode
{
    public enum NodeType
    {
        Location,
        NPC,
        Hero,
        Combatant,
        Event
    }

    public class Node : MonoBehaviour
    {
        public Cell CurrentCell;
        private SpriteRenderer mySpriteRenderer;
        [SerializeField] protected int moveDistance = 1;
        [SerializeField] private bool willMoveTowardsPlayer = false;
        private bool isStunned;
        private int roundsStunned = 0;

        protected Rigidbody2D myRigidBody;

        [SerializeField] public NodeData NodeData;
        public CharacterAttributes Attributes { get; private set; }

        List<Node> usedNodes = new();

        public int MovesRemaining { get; private set; }
        public Action OnRemainingMovesChanged;

        void OnValidate()
        {
            if (NodeData != null)
            {
                mySpriteRenderer = GetComponent<SpriteRenderer>();
                mySpriteRenderer.sprite = NodeData.Sprite;
            }
        }

        protected virtual void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.sprite = NodeData.Sprite;

            myRigidBody = GetComponent<Rigidbody2D>();

            if (NodeData.AttributesData != null)
            {
                Attributes = new CharacterAttributes(NodeData.AttributesData);
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
            usedNodes = new();
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
            if (Attributes != null)
            {
                MovesRemaining = Math.Clamp(Attributes.GetAttributeValue(AttributeType.Speed), 1, 99);
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
                List<Cell> path = GameManager.Instance.Grid.GetPathBetweenTwoCells(this.CurrentCell, GameManager.Instance.Player.HeroNode.CurrentCell);
                MoveToCell(path[0]);
                if (CurrentCell == GameManager.Instance.Player.HeroNode.CurrentCell)
                {
                    isStunned = true;
                    roundsStunned = 2;
                }
            }
        }
    }
}
