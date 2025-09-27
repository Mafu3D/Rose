

using Project.Combat;
using Project.Stats;
using UnityEngine;

namespace Project.GameNode
{
    public class MonsterNode : CombatNode, IMovableNode
    {
        public override Status Process()
        {
            if (!BattleManager.Instance.IsActiveBattle)
            {
                BattleManager.Instance.StartNewBattle(GameManager.Instance.Hero, this);
            }
            return BattleManager.Instance.Process();
        }

        public override void Reset()
        {
            // Noop
        }

        public void Move(Vector2 direction)
        {
            // TOOD: Implement
        }
    }
}