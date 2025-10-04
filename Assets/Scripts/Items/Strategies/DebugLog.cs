
using Project.Items;
using UnityEngine;

namespace Project.CombatEffects
{
    [CreateAssetMenu(fileName = "NewDebugLog", menuName = "Combat Effects/Debug/Log", order = 1)]
    public class DebugLog : CombatEffectStrategy
    {
        [SerializeField] string ToSay;

        public override void EndEffect(Character user, Character target)
        {
        }

        public override void ResetEffect(Character user, Character target)
        {
        }


        public override Status ResolveEffect(Character user, Character target)
        {
            return Status.Complete;
        }

        public override Status StartEffect(Character user, Character target)
        {
            Debug.Log($"{user}: {ToSay}");
            return Status.Complete;
        }
    }
}