using System.Collections.Generic;
using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewNoEffect", menuName = "Effects/No Effect", order = 1)]
    public class NoEffect : GameplayEffectStrategy
    {
        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            return Status.Running;
        }
    }
}