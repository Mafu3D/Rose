using Project.Grid;
using UnityEngine;

public abstract class Node : MonoBehaviour
{
    public Cell CurrentCell;

    void Start()
    {
        CurrentCell = GridManager.Instance.WorldPositionToCell(this.transform.position);
    }

    public abstract void ProcessEnter();

    public abstract void Reset();
}
