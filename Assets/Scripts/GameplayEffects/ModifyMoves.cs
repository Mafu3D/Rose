using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewModifyMoves", menuName = "Effects/Modify Moves", order = 1)]
    public class ModifyMoves : GameplayEffectStrategy
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
            GameManager.Instance.Player.HeroTile.ModifyMovesRemaining(Amount);
            return Status.Complete;
        }
    }
}