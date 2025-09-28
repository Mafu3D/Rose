

using Project.Combat;
using Project.Attributes;
using UnityEngine;

namespace Project.GameNode
{
    public class MonsterNode : CombatNode, IMovableNode
    {
        bool hasSentBattleRequest;
        public override Status OnTurnResolve()
        {
            return Status.Complete;
            // base.OnTurnResolve();

            // if (!BattleManager.Instance.IsActiveBattle && !hasSentBattleRequest)
            // {
            //     BattleManager.Instance.StartNewBattle(GameManager.Instance.Hero, this);
            //     hasSentBattleRequest = true;
            // }

            // if (BattleManager.Instance.IsActiveBattle)
            // {
            //     return Status.Running;
            // }
            // else
            // {
            //     if (NodeData.DestroyAfterUsing)
            //     {
            //         GameManager.Instance.MarkNodeForDestroy(this);
            //     }
            //     return Status.Complete;
            // }
        }

        public void Move(Vector2 direction)
        {
            // TOOD: Implement
        }
    }
}