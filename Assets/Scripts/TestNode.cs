using Project.Grid;
using UnityEngine;

public class TestNode : Node
{
    public override void ProcessEnter()
    {
        Debug.Log("Entered node");
    }

    public override void Reset()
    {
        Debug.Log("Reset");
    }
}