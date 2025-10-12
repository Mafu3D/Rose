using Project.Attributes;
using Project.GameTiles;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewRestoreAllHealth", menuName = "Effects/Restore All Health", order = 1)]
    public class RestoreAllHealth : GameplayEffectStrategy
    {
        public override void ResetEffect() { }

        public override Status ResolveEffect()
        {
           return Status.Complete;
        }

        public override Status StartEffect()
        {
            Tile heroTile = GameManager.Instance.Player.HeroTile;
            int maxHealth = heroTile.Character.Attributes.GetAttribute(AttributeType.Health).GetMaxValue();
            heroTile.Character.Attributes.ModifyAttributeValue(AttributeType.Health, maxHealth);
            return Status.Complete;
        }
    }
}