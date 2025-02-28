using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    [Header("Tilemap & Ayarlar")]
    public Tilemap tilemap;          
    public TileBase targetTile;      // ( Sprite_Tiles_Elevation_6)

    [Header("Önizleme Tile'larý")]
    public TileBase allowedPreviewTile;    
    public TileBase disallowedPreviewTile;

    [Header("Kalýcý Tile")]
    public TileBase finalTile;     

    [Header("Yerleþtirme Modu")]
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
                    {
                        tilemap.SetTile(cellPos, allowedPreviewTile);
                    }
                    else
                    {
                        tilemap.SetTile(cellPos, disallowedPreviewTile);
                    }
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

            if (originalTiles.ContainsKey(clickedCell))
            {
                if (originalTiles[clickedCell] == targetTile)
                {
                    tilemap.SetTile(clickedCell, finalTile);
                    tilemap.RefreshTile(clickedCell);
                    Debug.Log("Tile yerleþtirildi: " + clickedCell);
                }
                else
                {
                    Debug.Log("Bu hücreye yerleþtirme yapýlamaz: " + clickedCell);
                }
            }
            RestoreAllTiles();
            isPlacing = false;
        }
    }

    private void RestoreAllTiles()
    {
        foreach (var kvp in originalTiles)
        {
            if (tilemap.GetTile(kvp.Key) != finalTile)
            {
                tilemap.SetTile(kvp.Key, kvp.Value);
                tilemap.RefreshTile(kvp.Key);
            }
        }
        originalTiles.Clear();
    }
}
