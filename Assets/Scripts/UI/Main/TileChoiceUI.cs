using System.Collections.Generic;
using Project.Core.GameEvents;
using Project.GameTiles;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class TileChoiceUI : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GameObject mainContainer;
        [SerializeField] private GameObject displayPrefab;
        [SerializeField] private List<RectTransform> displayParentTransforms;
        List<GameObject> displayedObjects = new();

        void Awake()
        {
            gameManager.OnGameStartEvent += Initialize;
        }

        void Initialize()
        {
            gameManager.GameEventManager.OnTileDrawStarted += DisplayObjects;
            gameManager.GameEventManager.OnTileDrawEnded += DestroyedDisplayedObjects;
        }

        private void DisplayObjects(IGameEvent gameEvent)
        {
            TileChoiceEvent tileChoiceEvent = gameEvent as TileChoiceEvent;
            List<TileData> tiles = tileChoiceEvent.Choice.GetAllItems();

            for (int i = 0; i < tiles.Count; i++)
            {
                PopulateDisplay(tiles[i], i + 1, displayParentTransforms[i]);
            }
            mainContainer.SetActive(true);
        }

        private void PopulateDisplay(TileData tile, int number, Transform parent)
        {
            GameObject displayedObeject = Instantiate(displayPrefab, parent.position, Quaternion.identity, parent);
            TileChoiceDisplay display = displayedObeject.GetComponent<TileChoiceDisplay>();
            display.RegisterDisplayTile(tile, number);
            displayedObjects.Add(displayedObeject);
        }

        private void DestroyedDisplayedObjects(IGameEvent _)
        {
            foreach (GameObject displayedCard in displayedObjects)
            {
                Destroy(displayedCard);
            }

            mainContainer.SetActive(false);
        }
    }
}