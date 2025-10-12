using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewWeaponUpgrade", menuName = "Effects/Weapon Upgrade", order = 1)]
    public class WeaponUpgrade : GameplayEffectStrategy
    {
        [SerializeField] ItemData itemData;

        public override void ResetEffect() { }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            Item item = new Item(itemData);
            GameManager.Instance.Player.HeroTile.Character.Inventory.AddWeaponUpgrade(item);
            return Status.Running;
        }
    }
}