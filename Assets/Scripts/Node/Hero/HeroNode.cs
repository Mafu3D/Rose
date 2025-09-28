using UnityEngine;
using Project.PlayerSystem;
using System;
using Project.Attributes;

namespace Project.GameNode.Hero
{
    public class HeroNode : CombatNode, IMovableNode
    {
        [SerializeField] public Player Player;
        [SerializeField] private int moveDistance = 1;

        private Rigidbody2D myRigidBody;

        public int MovesRemaining { get; private set; }

        public Action OnRemainingMovesChanged;

        protected override void Awake()
        {
            base.Awake();
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
            ResetMovesRemaining();
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

        public void ResetMovesRemaining()
        {
            MovesRemaining = Math.Clamp(Attributes.GetAttributeValue(AttributeType.Speed), 1, 99);
            OnRemainingMovesChanged?.Invoke();
        }
    }
}