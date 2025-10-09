namespace Project.GameTiles
{
    using UnityEngine;

    public class TileFactory : Singleton<TileFactory>
    {
        [SerializeField] GameObject tilePrefab;

        public void CreateTile(TileData tileData, Vector2 position)
        {
            GameObject gameObject = Instantiate(tilePrefab, position, Quaternion.identity);
            Tile tile = gameObject.GetComponent<Tile>();
            tile.SetTileData(tileData);

            tile.RegisterToGrid();
        }
    }
}