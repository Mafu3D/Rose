using System.Collections.Generic;
using Project.GameTiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class TileChoiceDisplay : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] TMP_Text displayName;
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] TMP_Text choiceNumber;
        [SerializeField] Image image;
        [SerializeField] Image background1;
        [SerializeField] Image background2;
        [Header("Icons")]
        [SerializeField] GameObject lockedIcon;
        [SerializeField] GameObject trappedIcon;
        [SerializeField] GameObject safeIcon;
        [SerializeField] GameObject dangerIcon;
        [SerializeField] List<GameObject> costIcons;

        public void DisplayTile(TileData tileData, int choiceNumber)
        {
            displayName.text = tileData.DisplayName;
            descriptionText.text = tileData.Description;
            image.sprite = tileData.Sprite;

            lockedIcon.SetActive(tileData.IsLocked);
            switch (tileData.DangerStatus)
            {
                case DangerStatus.Safe:
                    safeIcon.SetActive(true);
                    dangerIcon.SetActive(false);
                    break;
                case DangerStatus.Dangerous:
                    safeIcon.SetActive(false);
                    dangerIcon.SetActive(true);
                    break;
                default:
                    safeIcon.SetActive(false);
                    dangerIcon.SetActive(false);
                    break;

            }
            lockedIcon.SetActive(tileData.IsLocked);
            trappedIcon.SetActive(tileData.IsTrapped);

            this.choiceNumber.text = choiceNumber.ToString();

            string hexColor = "#222323";
            if (background1 != null)
            {
                switch (tileData.TileType)
                {
                    case TileType.Hero:
                    case TileType.Monster:
                        hexColor = "#222323";
                        break;
                    case TileType.NPC:
                        hexColor = "#5EA4DB";
                        break;
                    case TileType.Danger:
                        hexColor = "#B24242";
                        break;
                    case TileType.Economic:
                        hexColor = "#CAB742";
                        break;
                    case TileType.Event:
                        hexColor = "#F18A31";
                        break;
                    case TileType.Neutral:
                        hexColor = "#649061";
                        break;
                    case TileType.Special:
                        hexColor = "#C555A0";
                        break;

                }
                Color color;
                if (ColorUtility.TryParseHtmlString(hexColor, out color))
                {
                    background1.color = color;
                }
            }

            if (background2 != null)
            {
                string hexColor2 = hexColor;
                if (tileData.SecondaryTileType != TileType.None)
                {
                    switch (tileData.SecondaryTileType)
                    {
                        case TileType.Hero:
                        case TileType.Monster:
                            hexColor2 = "#222323";
                            break;
                        case TileType.NPC:
                            hexColor2 = "#5EA4DB";
                            break;
                        case TileType.Danger:
                            hexColor2 = "#B24242";
                            break;
                        case TileType.Economic:
                            hexColor2 = "#CAB742";
                            break;
                        case TileType.Event:
                            hexColor2 = "#F18A31";
                            break;
                        case TileType.Neutral:
                            hexColor2 = "#649061";
                            break;
                        case TileType.Special:
                            hexColor2 = "#C555A0";
                            break;

                    }
                }
                Color color;
                if (ColorUtility.TryParseHtmlString(hexColor2, out color))
                {
                    background2.color = color;
                }
            }
        }

    }
}