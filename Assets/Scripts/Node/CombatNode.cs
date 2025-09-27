using System;
using Project.Combat;
using Project.Stats;
using UnityEngine;

namespace Project.GameNode
{
    public class CombatNode : Node
    {
        [SerializeField] public StatsData statsData;
        public CharacterStats Stats;

        protected override void Awake()
        {
            base.Awake();
            Stats = new CharacterStats(statsData);
        }

        public override Status Resolve()
        {
            // Noop
            return Status.Running;
        }

        public override void Reset()
        {
            // Noop
        }

        public int GetHealthValue() => Stats.GetHealthValue();
        public int GetSpeedValue() => Stats.GetSpeedValue();
        public int GetStrengthValue() => Stats.GetStrengthValue();
        public int GetMagicValue() => Stats.GetMagicValue();
        public int GetDexterityValue() => Stats.GetDexterityValue();
        public int GetArmorValue() => Stats.GetArmorValue();

        public void Attack(out int attackValue)
        {
            attackValue = GetStrengthValue();
        }

        public void ReceiveAttack(HitReport hitReport)
        {
            Stats.DecreaseHealthValue(hitReport.Damage);
        }
    }
}