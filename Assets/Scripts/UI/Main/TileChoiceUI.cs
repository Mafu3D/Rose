using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.GameTiles;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class TileChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject nextTileDisplayPrefab;
        [SerializeField] private RectTransform centerTileDisplayTransform;
        [SerializeField] private RectTransform leftTileDisplayTransform;
        [SerializeField] private RectTransform rightTileDisplayTransform;

        List<GameObject> displayedTiles = new();

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.GameEventManager.OnTileDrawStarted += DisplayCards;
            gameManager.GameEventManager.OnTileDrawEnded += DestroyDisplayedCards;
        }

        private void DisplayCards(IGameEvent gameEvent)
        {
            TileChoiceEvent tileChoiceEvent = gameEvent as TileChoiceEvent;
            List<TileData> tiles = tileChoiceEvent.Choice.GetAllItems();

            // Single card
            if (tiles.Count == 1)
            {
                GameObject centerDisplayedTile = Instantiate(nextTileDisplayPrefab, centerTileDisplayTransform.position, Quaternion.identity, centerTileDisplayTransform);
                TileChoiceDisplay centerTileDisplay = centerDisplayedTile.GetComponent<TileChoiceDisplay>();
                centerTileDisplay.DisplayTile(tiles[0], 1);
                displayedTiles.Add(centerDisplayedTile);
            }

            // Choice of two
            else if (tiles.Count == 2)
            {
                GameObject displayedTileLeft = Instantiate(nextTileDisplayPrefab, leftTileDisplayTransform.position, Quaternion.identity, leftTileDisplayTransform);
                TileChoiceDisplay leftTileDisplay = displayedTileLeft.GetComponent<TileChoiceDisplay>();
                leftTileDisplay.DisplayTile(tiles[0], 1);
                displayedTiles.Add(displayedTileLeft);

                GameObject displayedTileRight = Instantiate(nextTileDisplayPrefab, rightTileDisplayTransform.position, Quaternion.identity, rightTileDisplayTransform);
                TileChoiceDisplay rightTileDisplay = displayedTileRight.GetComponent<TileChoiceDisplay>();
                rightTileDisplay.DisplayTile(tiles[1], 2);
                displayedTiles.Add(displayedTileRight);
            }

            else if (tiles.Count == 3)
            {
                GameObject displayedTileLeft = Instantiate(nextTileDisplayPrefab, leftTileDisplayTransform.position, Quaternion.identity, leftTileDisplayTransform);
                TileChoiceDisplay leftTileDisplay = displayedTileLeft.GetComponent<TileChoiceDisplay>();
                leftTileDisplay.DisplayTile(tiles[0], 1);
                displayedTiles.Add(displayedTileLeft);

                GameObject displayedTileCenter = Instantiate(nextTileDisplayPrefab,centerTileDisplayTransform.position, Quaternion.identity,centerTileDisplayTransform);
                TileChoiceDisplay centerTileDisplay = displayedTileCenter.GetComponent<TileChoiceDisplay>();
                centerTileDisplay.DisplayTile(tiles[1], 2);
                displayedTiles.Add(displayedTileCenter);

                GameObject displayedTileRight = Instantiate(nextTileDisplayPrefab, rightTileDisplayTransform.position, Quaternion.identity, rightTileDisplayTransform);
                TileChoiceDisplay rightTileDisplay = displayedTileRight.GetComponent<TileChoiceDisplay>();
                rightTileDisplay.DisplayTile(tiles[2], 3);
                displayedTiles.Add(displayedTileRight);
            }
        }

        private void DestroyDisplayedCards(IGameEvent _)
        {
            foreach (GameObject displayedCard in displayedTiles)
            {
                Destroy(displayedCard);
            }
        }
    }
}