using System;
using System.Collections.Generic;
using Project.Attributes;
using Project.GameplayEffects;
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

    public class Node : MonoBehaviour, IMovableNode
    {
        public Cell CurrentCell;
        private SpriteRenderer mySpriteRenderer;

        [SerializeField] public NodeData NodeData;
        public CharacterAttributes Attributes { get; private set; }

        [SerializeField] private int moveDistance = 1;

        private Rigidbody2D myRigidBody;

        public int MovesRemaining { get; private set; }

        public Action OnRemainingMovesChanged;


        List<Node> usedNodes = new();

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

            if (NodeData.AttributesData != null)
            {
                Attributes = new CharacterAttributes(NodeData.AttributesData);
            }

            myRigidBody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            ResetMovesRemaining();
            RegisterToGrid();
            ResetNode();
        }

        private void ResetNode()
        {
            if (NodeData.OnActivateStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnActivateStrategies) strategy.ResetEffect(this, null);
            if (NodeData.OnPlayerEnterStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerEnterStrategies) strategy.ResetEffect(this, null);
            if (NodeData.OnPlayerExitStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnPlayerExitStrategies) strategy.ResetEffect(this, null);
            if (NodeData.OnCreateStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnCreateStrategies) strategy.ResetEffect(this, null);
            if (NodeData.OnDestroyStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnDestroyStrategies) strategy.ResetEffect(this, null);
            if (NodeData.OnStartOfRoundStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnStartOfRoundStrategies) strategy.ResetEffect(this, null);
            if (NodeData.OnEndOfTurnStratgies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnEndOfTurnStratgies) strategy.ResetEffect(this, null);
            if (NodeData.OnEndOfRoundStrategies != null) foreach (GameplayEffectStrategy strategy in NodeData.OnEndOfRoundStrategies) strategy.ResetEffect(this, null);
            usedNodes = new();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }

        public void OnPlayerEnter()
        {
            ExecuteStrategies(NodeData.OnPlayerEnterStrategies);
        }

        public void ExecuteOnActivate()
        {
            ExecuteStrategies(NodeData.OnActivateStrategies);
        }

        public void ExecuteOnEndOfTurn()
        {
            ExecuteStrategies(NodeData.OnEndOfTurnStratgies);
        }

        private void ExecuteStrategies(List<GameplayEffectStrategy> strategies)
        {
            foreach (GameplayEffectStrategy effect in strategies)
            {
                GameManager.Instance.EffectQueue.QueueEffect(effect, this, null);
            }
        }

        public void ResetStrategies(List<GameplayEffectStrategy> strategies)
        {

        }

        public void Move(Vector2 direction)
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

        public void MoveToCell(Cell cell)
        {
            if (GameManager.Instance.Grid.TryGetCellInWalkableCells(cell))
            {
                CurrentCell = cell;
                myRigidBody.MovePosition(cell.Center);
                GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);

                MovesRemaining -= 1;

                OnRemainingMovesChanged?.Invoke();
            }
        }

        public void ResetMovesRemaining()
        {
            MovesRemaining = 0;
            if (Attributes != null)
            {
                MovesRemaining = Math.Clamp(Attributes.GetAttributeValue(AttributeType.Speed), 1, 99);
                OnRemainingMovesChanged?.Invoke();
            }
        }
    }
}
