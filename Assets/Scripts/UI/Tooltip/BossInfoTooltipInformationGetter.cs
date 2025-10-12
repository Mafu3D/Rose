using UnityEngine;

namespace Project.UI
{
    public class BossInfoTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] GameManager gameManager;

        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            CharacterData bossData = gameManager.Boss;
            if (bossData != null)
            {
                header = bossData.DisplayName;
                content = $"Health: {bossData.AttributesData.Health} \n";
                content += $"Armor: {bossData.AttributesData.Armor} \n";
                content += $"Power: {bossData.AttributesData.Strength} \n";
                content += $"{bossData.CombatDescription}";
                return true;
            }
            content = default;
            header = default;
            return false;
        }
    }
}
