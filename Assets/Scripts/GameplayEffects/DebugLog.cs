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

        public override void Reset() { }

        public override Status Resolve() => Status.Complete;

        public override Status Start()
        {
            Debug.Log(ToSay);
            return Status.Complete;
        }
    }
}