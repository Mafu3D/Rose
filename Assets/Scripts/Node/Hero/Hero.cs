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
            CurrentCell = GridManager.Instance.WorldPositionToCell(this.transform.position);
            GridManager.Instance.RegisterToCell(CurrentCell, this);
        }

        public void Move(Vector2 direction)
        {
            if (direction != Vector2.zero)
            {
                GridManager.Instance.DeregisterFromCell(CurrentCell, this);
                Cell destinationCell = GridManager.Instance.GetNeighborCell(CurrentCell, direction);
                CurrentCell = destinationCell;
                myRigidBody.MovePosition(destinationCell.Center);
                GridManager.Instance.RegisterToCell(CurrentCell, this);
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