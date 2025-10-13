using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Decks;
using Project.GameTiles;
using Project.Items;
using Project.PlayerSystem;
using TMPro;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class MainUI : Singleton<MainUI>
    {
        [SerializeField] GameObject activateButtonContainer;
        [SerializeField] TMP_Text activateText;

        private void Start()
        {
            activateButtonContainer.SetActive(false);
        }

        void Update()
        {
            if (!GameManager.Instance.GameEventManager.GameEventActive())
            {
                Cell heroCell = GameManager.Instance.Player.HeroTile.CurrentCell;
                List<Tile> registeredTiles;
                if (GameManager.Instance.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
                {
                    foreach (Tile tile in registeredTiles)
                    {
                        if (tile.CanActivate())
                        {
                            activateButtonContainer.SetActive(true);
                            activateText.text = $"[Space] Activate: {tile.TileData.DisplayName}";
                            return;
                        }
                    }
                }
            }
            activateButtonContainer.SetActive(false);
            activateText.text = "";
        }
    }
}