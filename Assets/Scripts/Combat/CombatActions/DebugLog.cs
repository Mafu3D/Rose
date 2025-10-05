using UnityEngine;

namespace Project.Combat.CombatActions
{
    [CreateAssetMenu(fileName = "NewDebugLog", menuName = "Combat Actions/Debug/Log", order = 1)]
    public class DebugLog : CombatActionBaseData
    {
        [SerializeField] string toSay;

        public override void Execute(Character user, Character target)
        {
            Debug.Log($"{user.DisplayName}: {toSay}");
        }

        protected override string Message(Character user, Character target)
        {
            return $"{user.DisplayName}: {toSay}";
        }
    }
}