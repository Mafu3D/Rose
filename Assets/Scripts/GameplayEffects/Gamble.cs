using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewGamble", menuName = "Effects/Gamble", order = 1)]
    public class Gamble : GameplayEffectStrategy
    {
        [SerializeField] int Amount;
        [Range(0, 100)]
        [SerializeField] float chanceToWin;

        public override void ResetEffect()
        {
        }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            float random = Random.Range(0, 100);

            if (random <= chanceToWin)
            {
                GameManager.Instance.Player.GoldTracker.AddGold(Amount * 2);
            }
            return Status.Complete;
        }
    }
}