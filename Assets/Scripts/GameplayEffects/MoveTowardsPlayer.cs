using System.Collections.Generic;
using Project.GameNode;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewMoveTowardsPlayer", menuName = "Effects/Move Towards Player", order = 1)]
    public class MoveTowardsPlayer : GameplayEffectStrategy
    {
        List<Cell> path = new();
        int pathIndex = 0;

        public override void ResetEffect(Node user, Node target)
        {
            path = new();
            pathIndex = 0;
        }

        public override Status ResolveEffect(Node user, Node target)
        {
            while (user.MovesRemaining > 0)
            {
                user.MoveToCell(path[pathIndex]);
                pathIndex++;
            }
            return Status.Complete;
        }

        public override Status StartEffect(Node user, Node target)
        {
            path = GameManager.Instance.Grid.GetPathBetweenTwoCells(user.CurrentCell, GameManager.Instance.Player.HeroNode.CurrentCell);
            return Status.Running;
        }
    }
}