

using Project.Combat;
using Project.Attributes;
using UnityEngine;

namespace Project.GameNode
{
    public class MonsterNode : CombatNode, IMovableNode
    {
        bool hasSentBattleRequest;
        public override Status Resolve()
        {
            if (!BattleManager.Instance.IsActiveBattle && !hasSentBattleRequest)
            {
                BattleManager.Instance.StartNewBattle(GameManager.Instance.Hero, this);
                hasSentBattleRequest = true;
            }

            if (BattleManager.Instance.IsActiveBattle)
            {
                return Status.Running;
            }
            else
            {
                return Status.Complete;
            }
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