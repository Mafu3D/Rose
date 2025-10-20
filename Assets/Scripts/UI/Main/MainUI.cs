using System.Collections.Generic;
using System.Linq;
using Project;
using Project.Decks;
using Project.GameTiles;
using Project.Items;
using Project.PlayerSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI.MainUI
{
    public class MainUI : Singleton<MainUI>
    {
        [SerializeField] GameObject activateButtonContainer;
        [SerializeField] TMP_Text activateText;
        [SerializeField] Button activateButton;
        [SerializeField] Button endTurnEarlyButton;

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
                int movesRemaining = GameManager.Instance.Player.HeroTile.MovesRemaining;
                if (GameManager.Instance.Grid.TryGetTileesRegisteredToCell(heroCell, out registeredTiles))
                {
                    foreach (Tile tile in registeredTiles)
                    {
                        if (tile.CanActivate() && movesRemaining > 0)
                        {
                            activateButtonContainer.SetActive(true);
                            activateText.text = $"End Turn Early: Use {tile.TileData.DisplayName} \n (Moves Remaining {movesRemaining})";
                            // activateButton.onClick.AddListener(OnEndTurnClicked);
                            endTurnEarlyButton.gameObject.SetActive(false);
                            return;
                        }
                    }
                }
                else if (movesRemaining > 0)
                {
                    endTurnEarlyButton.gameObject.SetActive(true);
                            return;
                }
            }
            endTurnEarlyButton.gameObject.SetActive(false);
            activateButtonContainer.SetActive(false);
            activateText.text = "";
        }

        public void OnEndTurnClicked()
        {
            GameManager.Instance.EndTurnEarly();
        }
    }
}