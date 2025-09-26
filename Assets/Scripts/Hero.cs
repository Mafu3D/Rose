using UnityEngine;
using Project.PlayerSystem;
using Project.Grid;

namespace Project.Hero
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] public Player Player;

        public Cell CurrentCell;

        private Rigidbody2D myRigidBody;
        private HeroData heroData => Player.HeroData;

        private float timeBetweenMoves => 1f / heroData.MoveSpeed;
        private float timeSinceLastMove = 0f;

        void Awake()
        {
            myRigidBody = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            CurrentCell = GridManager.Instance.WorldPositionToCell(this.transform.position);
            Debug.Log(CurrentCell.ToString());
        }

        void Update()
        {
            Move();
        }

        private void Move()
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove > timeBetweenMoves)
            {
                Vector2 movementValue = Player.InputReader.MovementValue;
                if (movementValue != Vector2.zero)
                {
                    Cell destinationCell = GridManager.Instance.GetNeighborCell(CurrentCell, movementValue);
                    CurrentCell = destinationCell;
                    myRigidBody.MovePosition(destinationCell.Center);
                    Debug.Log(CurrentCell.ToString());
                    timeSinceLastMove = 0f;

                    GameManager.Instance.ProcessTurn();
                }
            }
        }

        void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Node"))
            {
                Node node = collision.GetComponent<Node>();
                if (node != null)
                {
                    Debug.Log("here1");
                    if (node.CurrentCell == this.CurrentCell)
                    {
                        Debug.Log("here2");
                        node.ProcessEnter();
                    }
                }
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Node"))
            {
                // CollisionType collisionType = CollisionType.Adjacent;
                // if (this.transform.position == collision.transform.position)
                // {
                //     collisionType = CollisionType.Overlap;
                // }

                // INode node = collision.GetComponent<INode>();
                // if (node != null)
                // {
                //     if (collisionType == CollisionType.Overlap)
                //     {
                //         node.Reset();
                //     }
                // }
            }
        }
    }
}