using Project.Grid;
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

    public enum Status
    {
        Running,
        Success,
        Failure
    }

    public abstract class Node : MonoBehaviour
    {
        public Cell CurrentCell;
        private SpriteRenderer mySpriteRenderer;

        [SerializeField] public NodeData NodeData;

        public abstract Status Process();

        public abstract void Reset();

        protected virtual void Awake()
        {
            mySpriteRenderer = GetComponent<SpriteRenderer>();
            mySpriteRenderer.sprite = NodeData.Sprite;
        }

        protected virtual void Start()
        {
            RegisterToGrid();
        }

        public void RegisterToGrid()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }
    }
}
