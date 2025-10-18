using Project.Items;
using UnityEngine;

namespace Project.GameplayEffects
{
    [CreateAssetMenu(fileName = "NewAddItem", menuName = "Effects/Add Item", order = 1)]
    public class AddItem : GameplayEffectStrategy
    {
        [SerializeField] ItemData itemData;
        [SerializeField] bool addToSecretItems;

        public override void ResetEffect() { }

        public override Status ResolveEffect()
        {
            return Status.Complete;
        }

        public override Status StartEffect()
        {
            Item item = new Item(itemData);
            if (!addToSecretItems)
            {
                switch (itemData.ItemType)
                {
                    case ItemType.Weapon:
                        GameManager.Instance.Player.HeroTile.Character.Inventory.SwapEquippedWeapon(item);
                        break;
                    case ItemType.Offhand:
                        GameManager.Instance.Player.HeroTile.Character.Inventory.SwapEquippedOffhand(item);
                        break;
                    default:
                        GameManager.Instance.Player.HeroTile.Character.Inventory.AddItem(item);
                        break;
                }
            }
            else
            {
                GameManager.Instance.Player.HeroTile.Character.Inventory.AddSecretItem(item);
            }
            return Status.Running;
        }
    }
}