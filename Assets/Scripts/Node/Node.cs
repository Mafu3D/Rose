using Project.Grid;
using UnityEngine;

namespace Project.GameNode
{
    public enum Status
    {
        Running,
        Success,
        Failure
    }

    public abstract class Node : MonoBehaviour
    {
        public Cell CurrentCell;

        [SerializeField] public NodeData NodeData;

        public abstract Status Process();

        public abstract void Reset();

        protected virtual void Start()
        {
            CurrentCell = GameManager.Instance.Grid.WorldPositionToCell(this.transform.position);
            GameManager.Instance.Grid.RegisterToCell(CurrentCell, this);
        }
    }
}
