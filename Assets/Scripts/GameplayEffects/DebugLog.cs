using System.Collections.Generic;
using Project.GameTiles;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewDebugLog", menuName = "Effects/Debug/Log", order = 1)]
    public class DebugLog : GameplayEffectStrategy
    {
        [SerializeField] string ToSay;

        public override void ResetEffect() { }

        public override Status ResolveEffect() => Status.Complete;

        public override Status StartEffect()
        {
            Debug.Log(ToSay);
            return Status.Complete;
        }
    }
}