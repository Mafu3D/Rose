using UnityEngine;
using Project.PlayerSystem;
using Project.Grid;
using Project.GameNode;

namespace Project.GameNode.Hero
{
    public class HeroNode : Node
    {
        [SerializeField] public Player Player;

        private Rigidbody2D myRigidBody;
        private HeroData heroData => Player.HeroData;

        void Awake()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                GameManager.Instance.Grid.DeregisterFromCell(CurrentCell, this);
                Cell destinationCell = GameManager.Instance.Grid.GetNeighborCell(CurrentCell, direction);
                CurrentCell = destinationCell;
                myRigidBody.MovePosition(destinationCell.Center);
                GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
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