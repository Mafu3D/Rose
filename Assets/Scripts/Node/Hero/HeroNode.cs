using UnityEngine;
using Project.PlayerSystem;
using Project.Grid;
using Project.GameNode;
using System;
using Project.Stats;

namespace Project.GameNode.Hero
{
    public class HeroNode : Node
    {
        [SerializeField] public Player Player;

        private Rigidbody2D myRigidBody;
        public HeroData HeroData => Player.HeroData;
        public CharacterStats Stats;

        public int MovesRemaining { get; private set; }

        public Action OnRemainingMovesChanged;

        protected override void Awake()
        {
            base.Awake();
            Stats = new CharacterStats(HeroData.StatsData);
            myRigidBody = GetComponent<Rigidbody2D>();
            ResetMovesRemaining();
        }

        protected override void Start()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero && MovesRemaining > 0)
            {
                GameManager.Instance.Grid.DeregisterFromCell(CurrentCell, this);
                Cell destinationCell = GameManager.Instance.Grid.GetNeighborCell(CurrentCell, direction * HeroData.MoveDistance);
                CurrentCell = destinationCell;
                myRigidBody.MovePosition(destinationCell.Center);
                GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);

                MovesRemaining -= 1;

                OnRemainingMovesChanged?.Invoke();
            }
        }

        public void ResetMovesRemaining()
        {
            MovesRemaining = Stats.GetSpeedValue();
            OnRemainingMovesChanged?.Invoke();
        }

        public override Status Process()
        {
            return Status.Success;
        }

        public override void Reset()
        {
            //Noop
        }
    }
}