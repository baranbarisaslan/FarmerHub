using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    public Tilemap tilemap;
    public TileBase targetTile;
    public TileBase allowedPreviewTile;
    public TileBase disallowedPreviewTile;
    public TileBase finalTile;
    public TileBase houseTile;
    public TileBase towerTile;

    public GameObject housePrefab;
    public GameObject towerPrefab;

    public bool isPlacing = false;

    private Dictionary<Vector3Int, TileBase> originalTiles = new Dictionary<Vector3Int, TileBase>();

    public void StartPlacing()
    {
        isPlacing = true;
        originalTiles.Clear();

        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                TileBase t = tilemap.GetTile(cellPos);
                if (t != null)
                {
                    originalTiles[cellPos] = t;
                    if (t == targetTile)
                        tilemap.SetTile(cellPos, allowedPreviewTile);
                    tilemap.RefreshTile(cellPos);
                }
            }
        }
    }

    void Update()
    {
        if (!isPlacing)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int clickedCell = tilemap.WorldToCell(mouseWorldPos);

            if (!originalTiles.ContainsKey(clickedCell))
                return;

            if (finalTile == houseTile || finalTile == towerTile)
            {
                List<Vector3Int> houseCells = new List<Vector3Int>
                {
                    clickedCell,
                    clickedCell + new Vector3Int(1, 0, 0),
                    clickedCell + new Vector3Int(0, 1, 0),
                    clickedCell + new Vector3Int(1, 1, 0)
                };

                bool canPlace = true;
                foreach (Vector3Int cell in houseCells)
                {
                    if (!originalTiles.ContainsKey(cell) || originalTiles[cell] != targetTile)
                    {
                        canPlace = false;
                        break;
                    }
                }

                if (canPlace)
                {
                    if (finalTile == towerTile)
                    {
                        foreach (Vector3Int cell in houseCells)
                        {
                            tilemap.SetTile(cell, towerTile);
                            tilemap.RefreshTile(cell);
                            originalTiles.Remove(cell);
                        }
                        Vector3 houseWorldPos = tilemap.CellToWorld(clickedCell);
                        GameObject newTower = Instantiate(towerPrefab, houseWorldPos, Quaternion.identity);
                        newTower.transform.position += new Vector3(1f, 1.75f, 0);
                    }
                    else
                    {
                        foreach (Vector3Int cell in houseCells)
                        {
                            tilemap.SetTile(cell, houseTile);
                            tilemap.RefreshTile(cell);
                            originalTiles.Remove(cell);
                        }
                        Vector3 houseWorldPos = tilemap.CellToWorld(clickedCell);
                        GameObject newHouse = Instantiate(housePrefab, houseWorldPos, Quaternion.identity);
                        newHouse.transform.position += new Vector3(1f, 1.75f, 0);
                    }
                }
                EndPlacing();
            }
            else
            {
                if (originalTiles[clickedCell] == targetTile)
                {
                    tilemap.SetTile(clickedCell, finalTile);
                    tilemap.RefreshTile(clickedCell);
                    originalTiles.Remove(clickedCell);
                }
                else
                {
                    Debug.Log("Bu hücreye yerleþtirme yapýlamaz: " + clickedCell);
                }
            }
        }
    }

    public void EndPlacing()
    {
        foreach (var kvp in originalTiles)
        {
            tilemap.SetTile(kvp.Key, kvp.Value);
            tilemap.RefreshTile(kvp.Key);
        }
        originalTiles.Clear();
        isPlacing = false;
    }
}
