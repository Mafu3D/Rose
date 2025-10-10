using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.GameLoop;
using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewChooseTreasure", menuName = "Effects/Choose Treasure", order = 1)]
    public class ChooseTreaure : GameplayEffectStrategy
    {
        [SerializeField] int amount = 3;
        [SerializeField] bool shuffleRemaining = true;

        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            if (GameManager.Instance.IsChoosingTreasure) return Status.Running;
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            GameManager.Instance.GameEventManager.StartItemDrawEvent(amount, shuffleRemaining);
            GameManager.Instance.StateMachine.SwitchState(new SelectingItemState("Selecting Item State", GameManager.Instance.StateMachine, GameManager.Instance));

            return Status.Running;
        }
    }
}