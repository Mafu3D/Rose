using Project.GameLoop;
using UnityEngine;

namespace Project.GameplayEffects
{


    [CreateAssetMenu(fileName = "NewOpenShop", menuName = "Effects/Open Shop", order = 1)]
    public class OpenShop : GameplayEffectStrategy
    {
        [SerializeField] private int inventoryAmount = 8;
        [SerializeField] private float priceModifier = 1;
        [SerializeField] private bool replaceOnBuy = false;
        [SerializeField] private bool refreshable = true;
        [SerializeField] private int refreshCost = 5;

        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            if (GameManager.Instance.GameEventManager.CurrentGameEvent != null) return Status.Running;
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            GameManager.Instance.GameEventManager.StartShopEvent(inventoryAmount, priceModifier, replaceOnBuy, refreshable, refreshCost);
            GameManager.Instance.StateMachine.SwitchState(new ShopState("Shopping State", GameManager.Instance.StateMachine, GameManager.Instance));

            return Status.Running;
        }
    }
}