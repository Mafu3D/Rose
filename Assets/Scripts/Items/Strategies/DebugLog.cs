using System.Collections.Generic;
using Project.Attributes;
using Project.Combat;
using Project.GameNode;
using Project.GameNode.Hero;
using Project.Items;
using Project.States;
using UnityEngine;

namespace Project.CombatEffects
{
    [CreateAssetMenu(fileName = "NewDebugLog", menuName = "Combat Effects/Debug/Log", order = 1)]
    public class DebugLog : CombatEffectStrategy
    {
        [SerializeField] string ToSay;

        public override void EndEffect(Combatant user, Combatant target)
        {
        }

        public override void ResetEffect(Combatant user, Combatant target)
        {
        }


        public override Status ResolveEffect(Combatant user, Combatant target)
        {
            return Status.Complete;
        }

        public override Status StartEffect(Combatant user, Combatant target)
        {
            Debug.Log($"{user}: {ToSay}");
            return Status.Complete;
        }
    }
}