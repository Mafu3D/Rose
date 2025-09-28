using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewNoEffect", menuName = "Effects/No Effect", order = 1)]
    public class NoEffect : GameplayEffectStrategy
    {
        public override void Reset()
        {
        }

        public override Status Resolve()
        {
            return Status.Complete;
        }

        public override Status Start()
        {
            return Status.Running;
        }
    }
}