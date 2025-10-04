using System.Collections.Generic;
using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewMoveTile", menuName = "Effects/Move Tile", order = 1)]
    public class MoveTile : GameplayEffectStrategy
    {
        public override void ResetEffect() { }

        public override Status ResolveEffect() => Status.Complete;

        public override Status StartEffect()
        {
            return Status.Complete;
        }
    }
}