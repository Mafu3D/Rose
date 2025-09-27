using Project.Combat;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

public interface ICombatantNode
{
    public int GetHealthValue();
    public int GetSpeedValue();
    public int GetStrengthValue();
    public int GetMagicValue();
    public int GetDexterityValue();
    public int GetArmorValue();

    public void Attack(out int attackValue);

    public void ReceiveAttack(HitReport hitReport);
}