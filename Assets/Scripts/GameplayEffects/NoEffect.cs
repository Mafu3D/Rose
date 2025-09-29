using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewNoEffect", menuName = "Effects/No Effect", order = 1)]
    public class NoEffect : GameplayEffectStrategy
    {
        public override void ResetEffect(Node user, Node target)
        {
        }

        public override Status ResolveEffect(Node user, Node target)
        {
            return Status.Complete;
        }

        public override Status StartEffect(Node user, Node target)
        {
            Debug.Log($"{user.name} performed no effect.");
            return Status.Running;
        }
    }
}