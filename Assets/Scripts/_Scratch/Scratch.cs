using Project;
using Project.Grid;
using UnityEngine;

public class Scratch : MonoBehaviour {
    [SerializeField] GameObject hitPointObject;
    bool clicked = false;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!clicked)
            {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Cell hitCell = GameManager.Instance.Grid.WorldPositionToCell(worldPosition);
                Instantiate(hitPointObject, worldPosition, Quaternion.identity);
                Debug.Log($"X: {hitCell.x} Y: {hitCell.y}");
                Debug.Log(hitCell.Center);
                // Debug.Log(hitCell.GetHashCode());

                clicked = true;
            }
        }
        else
        {
            clicked = false;
        }
    }
}