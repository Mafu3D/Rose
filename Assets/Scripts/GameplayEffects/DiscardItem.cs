using Project.GameLoop;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewDiscardItem", menuName = "Effects/Discard Item", order = 1)]
    public class DiscardItem : GameplayEffectStrategy
    {
        [SerializeField] private int amount = 1;

        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            if (GameManager.Instance.GameEventManager.CurrentInventoryChoiceEvent != null) return Status.Running;
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            GameManager.Instance.GameEventManager.StartInventoryChoiceEvent(amount);
            GameManager.Instance.StateMachine.SwitchState(new DiscardingItemState("Discarding Item State", GameManager.Instance.StateMachine, GameManager.Instance));

            return Status.Running;
        }
    }
}