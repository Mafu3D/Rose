namespace Project.Stats
{
    public class CharacterStats
    {
        Stat Health;
        Stat Speed;
        Stat Strength;
        Stat Magic;
        Stat Dexterity;
        Stat Armor;

        public CharacterStats(StatsData data)
        {
            this.Health = new Stat("Health", data.Health);
            this.Speed = new Stat("Speed", data.Speed);
            this.Strength = new Stat("Strength", data.Strength);
            this.Magic = new Stat("Magic", data.Magic);
            this.Dexterity = new Stat("Dexterity", data.Dexterity);
            this.Armor = new Stat("Armor", data.Armor);
        }

        public int GetHealthValue() => Health.GetValue();
        public int GetSpeedValue() => Speed.GetValue();
        public int GetStrengthValue() => Strength.GetValue();
        public int GetMagicValue() => Magic.GetValue();
        public int GetDexterityValue() => Dexterity.GetValue();
        public int GetArmorValue() => Armor.GetValue();
    }
}