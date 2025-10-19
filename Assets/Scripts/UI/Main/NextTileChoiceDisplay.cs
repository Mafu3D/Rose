using Project.GameTiles;
using UnityEngine;

namespace Project.UI.MainUI
{
    public class NextTileChoiceDisplay : MonoBehaviour
    {
        [SerializeField] GameObject tileChoiceContainerPrefab;
        [SerializeField] GameObject tileChoice01Container;
        [SerializeField] GameObject tileChoice02Container;
        [SerializeField] GameObject tileChoice03Container;

        public void DisplayChoices(Choice<TileData> tileChoice)
        {
            GameObject itemChoice01 = Instantiate(tileChoiceContainerPrefab, tileChoice01Container.transform.position, Quaternion.identity, tileChoice01Container.transform);
            TileChoiceDisplay itemChoiceDisplay01 = itemChoice01.GetComponent<TileChoiceDisplay>();
            itemChoiceDisplay01.RegisterDisplayTile(tileChoice.GetItem(0), 1);

            GameObject itemChoice02 = Instantiate(tileChoiceContainerPrefab, tileChoice02Container.transform.position, Quaternion.identity, tileChoice02Container.transform);
            TileChoiceDisplay itemChoiceDisplay02 = itemChoice02.GetComponent<TileChoiceDisplay>();
            itemChoiceDisplay02.RegisterDisplayTile(tileChoice.GetItem(1), 2);

            GameObject itemChoice03 = Instantiate(tileChoiceContainerPrefab, tileChoice03Container.transform.position, Quaternion.identity, tileChoice03Container.transform);
            TileChoiceDisplay itemChoiceDisplay03 = itemChoice03.GetComponent<TileChoiceDisplay>();
            itemChoiceDisplay03.RegisterDisplayTile(tileChoice.GetItem(2), 3);

        }
    }
}