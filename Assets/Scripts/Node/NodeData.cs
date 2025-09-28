using System.Collections.Generic;
using Project.Attributes;
using Project.GameNode.Strategies;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Project.GameNode
{
    [CreateAssetMenu(fileName = "NodeData", menuName = "NodeData", order = 0)]
    public class NodeData : SerializedScriptableObject
    {
        [SerializeField] public string DisplayName;
        [SerializeField] public string Description;
        [SerializeField] public Sprite Sprite;
        [SerializeField] public int Priority = 0;
        [SerializeField] public NodeType NodeType;

        [SerializeField] public List<IStrategy> OnTurnResolveStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnPlayerEnterStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnPlayerExitStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnCreateStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnDestroyStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnRoundStartStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnPlayerTurnEndStrategies = new List<IStrategy>();
        [SerializeField] public List<IStrategy> OnRoundEndStrategies = new List<IStrategy>();

        // On PlayerEnter Strategies

        // On PlayerExit Strategies

        // On Resolve Strategies

        // On Create Strategies

        // On Destroy Strategies

        // On RoundStart Strategies

        // On PlayerTurnEnd Strategies

        // On RoundEnd Strategies

        // On Move Strategies
    }
}