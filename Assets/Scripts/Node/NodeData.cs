using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "NodeData", order = 0)]
public class NodeData : ScriptableObject {
    [SerializeField] public int Priority = 0;
}