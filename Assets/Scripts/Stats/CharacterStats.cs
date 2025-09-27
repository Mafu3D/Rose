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
        public void IncreaseHealthValue(int amount) => Health.IncreaseStat(amount);
        public void DecreaseHealthValue(int amount) => Health.DecreaseStat(amount);

        public int GetSpeedValue() => Speed.GetValue();
        public void IncreaseSpeedValue(int amount) => Speed.IncreaseStat(amount);
        public void DecreaseSpeedValue(int amount) => Speed.DecreaseStat(amount);


        public int GetStrengthValue() => Strength.GetValue();
        public void IncreaseStrengthValue(int amount) => Strength.IncreaseStat(amount);
        public void DecreaseStrengthValue(int amount) => Strength.DecreaseStat(amount);


        public int GetMagicValue() => Magic.GetValue();
        public void IncreaseMagicValue(int amount) => Magic.IncreaseStat(amount);
        public void DecreaseMagicValue(int amount) => Magic.DecreaseStat(amount);


        public int GetDexterityValue() => Dexterity.GetValue();
        public void IncreaseDexterityValue(int amount) => Dexterity.IncreaseStat(amount);
        public void DecreaseDexterityValue(int amount) => Dexterity.DecreaseStat(amount);


        public int GetArmorValue() => Armor.GetValue();
        public void IncreaseArmorValue(int amount) => Armor.IncreaseStat(amount);
        public void DecreaseArmorValue(int amount) => Armor.DecreaseStat(amount);


    }
}