using UnityEngine;

namespace Project.Combat.CombatActions
{
    [CreateAssetMenu(fileName = "NewDebugLog", menuName = "Combat Actions/Debug/Log", order = 1)]
    public class DebugLog : CombatActionBaseData
    {
        [SerializeField] string toSay;

        public override void Execute(Combatant user, Combatant target)
        {
            Debug.Log($"{user.DisplayName}: {toSay}");
            Debug.Log($"Test Action: {user.DisplayName} threw a bomb at {target.DisplayName}, dealing {toSay} damage!");
        }
    }
}