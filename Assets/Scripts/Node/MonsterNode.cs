

using Project.Combat;
using Project.Stats;
using UnityEngine;

namespace Project.GameNode
{
    public class MonsterNode : Node, ICombatantNode
    {
        public CharacterStats Stats;

        public Battle battle;

        protected override void Awake()
        {
            base.Awake();
            Stats = new CharacterStats(NodeData.StatsData);
        }
        public override Status Process()
        {
            if (battle == null)
            {
                battle = new Battle(GameManager.Instance.Hero, this);
                battle.StartBattle();
                BattleUI.Instance.OpenBattleUI(battle, GameManager.Instance.Hero.HeroData, this.NodeData);
                return Status.Running;
            }

            switch (battle.ProcessBattle())
            {
                case Status.Running:
                    break;
                case Status.Success:
                    BattleUI.Instance.CloseBattleUI(battle);
                    return Status.Success;
                default:
                    return Status.Failure;
            }
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