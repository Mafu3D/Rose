using UnityEngine;
using Project.PlayerSystem;
using Project.Grid;
using Project.GameNode;

namespace Project.Hero
{
    public class HeroNode : Node
    {
        [SerializeField] public Player Player;

        private Rigidbody2D myRigidBody;
        private HeroData heroData => Player.HeroData;

        private float timeBetweenMoves => 1f / heroData.MoveSpeed;
        private float timeSinceLastMove = 0f;

        void Awake()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            CurrentCell = GridManager.Instance.WorldPositionToCell(this.transform.position);
            GridManager.Instance.RegisterToCell(CurrentCell, this);
        }

        void Update()
        {
            if (GameManager.Instance.GameState == GameState.PlayerMove)
            {
                Move();
            }
        }

        private void Move()
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > timeBetweenMoves)
            {
                Vector2 movementValue = Player.InputReader.MovementValue;
                if (movementValue != Vector2.zero)
                {
                    GridManager.Instance.DeregisterFromCell(CurrentCell, this);
                    Cell destinationCell = GridManager.Instance.GetNeighborCell(CurrentCell, movementValue);
                    CurrentCell = destinationCell;
                    myRigidBody.MovePosition(destinationCell.Center);
                    GridManager.Instance.RegisterToCell(CurrentCell, this);
                    timeSinceLastMove = 0f;

                    GameManager.Instance.EndPlayerTurn();
                }
            }
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