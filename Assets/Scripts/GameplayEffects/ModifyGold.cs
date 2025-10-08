using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyGold", menuName = "Effects/Modify Gold", order = 1)]
    public class ModifyGold : GameplayEffectStrategy
    {
        [SerializeField] int Amount;
        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            if (Amount > 0)
            {
                GameManager.Instance.Player.GoldTracker.AddGold(Amount);
            }
            else if (Amount < 0)
            {
                GameManager.Instance.Player.GoldTracker.RemoveGold(Amount);
            }
            return Status.Complete;
        }
    }
}