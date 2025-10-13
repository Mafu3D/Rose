using Project.GameTiles;
using UnityEngine;

namespace Project.UI
{
    public class TileBoardTooltipInformationGetter : TooltipInformationGetter
    {
        [SerializeField] Tile tile;

        public override bool TryGetTooltipInformation(out string content, out string header)
        {
            TileData tileData = tile.TileData;

            if (tileData != null)
            {
                header = tileData.DisplayName;
                content = tileData.Description;
                return true;
            }

            content = default;
            header = default;
            return false;
        }
    }
}
