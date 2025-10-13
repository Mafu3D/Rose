using Project;
using UnityEngine;

public class TempRemoveItem : MonoBehaviour
{
    public void RemoveItem(int index)
    {
        GameManager.Instance.Player.HeroTile.Character.Inventory.RemoveItem(index);
    }

    public void RemoveWeapon()
    {
        GameManager.Instance.Player.HeroTile.Character.Inventory.RemoveWeapon();
    }

    public void RemoveOffhand()
    {
        GameManager.Instance.Player.HeroTile.Character.Inventory.RemoveOffhand();
    }
}