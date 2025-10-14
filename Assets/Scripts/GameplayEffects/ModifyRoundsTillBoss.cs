using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyRoundsTillBoss", menuName = "Effects/Modify Rounds Till Boss", order = 1)]
    public class ModifyRoundsTillBoss : GameplayEffectStrategy
    {
        [SerializeField] int amount;
        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            GameManager.Instance.RoundsTillBoss += amount;
            return Status.Running;
        }
    }
}