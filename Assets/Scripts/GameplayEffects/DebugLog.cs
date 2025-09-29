using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewDebugLog", menuName = "Effects/Debug/Log", order = 1)]
    public class DebugLog : GameplayEffectStrategy
    {
        [SerializeField] string ToSay;

        public override void ResetEffect(Node user, Node target) { }

        public override Status ResolveEffect(Node user, Node target) => Status.Complete;

        public override Status StartEffect(Node user, Node target)
        {
            Debug.Log($"{user.name}: {ToSay}");
            return Status.Complete;
        }
    }
}