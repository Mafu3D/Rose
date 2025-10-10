using Project.GameTiles;
using Project.Items;
using Project.UI.MainUI;
using UnityEngine;

namespace Project.UI
{
    public enum TileHelpIconType
    {
        Cost,
        Locked,
        DangerStatus,
        Trapped
    }

    public class TileIconTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] TileHelpIconType type;
        [SerializeField] TileChoiceDisplay tileChoiceDisplay;

        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            TileData tileData = tileChoiceDisplay.TileData;

            switch (type)
            {
                case TileHelpIconType.Cost:
                    if (tileData.Cost > 0)
                    {
                        header = "Gems";
                        content = $"This tile costs {tileData.Cost} gems to raise.";
                        return true;
                    }
                    break;
                case TileHelpIconType.Locked:
                    if (tileData.IsLocked)
                    {
                        header = "Locked";
                        content = "You may only draft this tile if you have a key to open it.";
                        return true;
                    }
                    break;
                case TileHelpIconType.DangerStatus:
                    switch (tileData.DangerStatus)
                    {
                        case DangerStatus.Safe:
                            header = "Safe";
                            content = "You will not encounter any monsters here.";
                            return true;
                        case DangerStatus.Dangerous:
                            header = "Danger!";
                            content = "You will always encounter a monster here.";
                            return true;
                        case DangerStatus.Standard:
                            header = "Risky";
                            content = "You have a chance to encounter a monster here.";
                            return true;
                    }
                    break;
                case TileHelpIconType.Trapped:
                    if (tileData.IsTrapped)
                    {
                        header = "Trapped!";
                        content = "If you do not have a disarm kit, you will take 3 damage from the trap here!";
                        return true;
                    }
                    break;
            }

            content = default;
            header = default;
            return false;
        }
    }
}
