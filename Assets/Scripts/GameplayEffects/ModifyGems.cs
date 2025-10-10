using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyGems", menuName = "Effects/Modify Gems", order = 1)]
    public class ModifyGems : GameplayEffectStrategy
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
                GameManager.Instance.Player.GemTracker.AddGem(Amount);
            }
            else if (Amount < 0)
            {
                GameManager.Instance.Player.GemTracker.RemoveGem(Amount);
            }
            return Status.Complete;
        }
    }
}