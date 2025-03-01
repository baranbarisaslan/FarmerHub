using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HousePlacementManager : MonoBehaviour
{
    private TileChanger tileChanger;
    private Tilemap tilemap;
    private bool isRepositioning = false;
    private Vector3Int currentHouseBaseCell;
    private List<Vector3Int> oldHouseCells = new List<Vector3Int>();

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    void Start()
    {
        tileChanger = FindFirstObjectByType<TileChanger>();
        tilemap = tileChanger.tilemap;
        currentHouseBaseCell = tilemap.WorldToCell(transform.position);
        oldHouseCells = new List<Vector3Int>
        {
            currentHouseBaseCell,
            currentHouseBaseCell + new Vector3Int(-1, 0, 0),
            currentHouseBaseCell + new Vector3Int(0, -1, 0),
            currentHouseBaseCell + new Vector3Int(-1, -1, 0)
        };
    }

    void Update()
    {
        if (isRepositioning && Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int newBaseCell = tilemap.WorldToCell(mouseWorldPos);

            List<Vector3Int> newHouseCells = new List<Vector3Int>
            {
                newBaseCell,
                newBaseCell + new Vector3Int(1, 0, 0),
                newBaseCell + new Vector3Int(0, 1, 0),
                newBaseCell + new Vector3Int(1, 1, 0)
            };

            bool canPlace = true;
            foreach (Vector3Int cell in newHouseCells)
            {
                if (tilemap.GetTile(cell) != tileChanger.targetTile)
                {
                    canPlace = false;
                    break;
                }
            }

            if (canPlace)
            {
                foreach (Vector3Int cell in oldHouseCells)
                {
                    tilemap.SetTile(cell, tileChanger.targetTile);
                }
                tilemap.RefreshAllTiles();

                foreach (Vector3Int cell in newHouseCells)
                {
                    tilemap.SetTile(cell, tileChanger.houseTile);
                }
                tilemap.RefreshAllTiles();

                transform.position = tilemap.CellToWorld(newBaseCell) + new Vector3(1f, 1.75f, 0);
                currentHouseBaseCell = newBaseCell;
                oldHouseCells = newHouseCells;
                isRepositioning = false;
                tileChanger.isPlacing = false;
                ObjectHelper.SetOpacity(gameObject, 1f);

            }
        }
    }

    void OnMouseDown()
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            if (!isRepositioning)
            {
                isRepositioning = true;
                tileChanger.StartPlacing();
                ObjectHelper.SetOpacity(gameObject, 0.5f);
            }
        }
        lastClickTime = Time.time;
    }
}
