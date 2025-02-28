using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    public Tilemap tilemap;                   // Tilemap_Farm, Inspector'da atay�n
    public TileBase targetTile;               // De�i�tirilmesi gereken tile (�rn. Sprite_Tiles_Elevation_6)

    [Header("�nizleme Tile'lar�")]
    public TileBase allowedPreviewTile;       // �zin verilen alan i�in (�r. ye�il)
    public TileBase disallowedPreviewTile;    // �zin verilmeyen alan i�in (�r. k�rm�z�)

    [Header("Kal�c� Tile")]
    public TileBase finalTile;                // T�klama sonras� kal�c� tile

    [Header("Yerle�tirme Modu")]
    public bool isPlacing = false;            // Yerle�tirme modu aktif mi?

    // H�crelerin orijinal tile'lar�n� saklar
    private Dictionary<Vector3Int, TileBase> originalTiles = new Dictionary<Vector3Int, TileBase>();

    public void StartPlacing()
    {
        isPlacing = true;
        originalTiles.Clear();

        // Tilemap'in t�m h�crelerini tar�yoruz
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
                    // Sadece targetTile i�in allowed, di�erleri i�in disallowed �nizleme uygula
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

        // Kullan�c� t�klad���nda yaln�zca t�klanan h�cre kontrol edilsin
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int clickedCell = tilemap.WorldToCell(mouseWorldPos);

            if (originalTiles.ContainsKey(clickedCell))
            {
                // Sadece orijinal tile targetTile ise kal�c� tile yerle�tirilsin
                if (originalTiles[clickedCell] == targetTile)
                {
                    tilemap.SetTile(clickedCell, finalTile);
                    tilemap.RefreshTile(clickedCell);
                    Debug.Log("Tile yerle�tirildi: " + clickedCell);
                    // Bu h�cre art�k restorasyona dahil edilmeyecek
                    originalTiles.Remove(clickedCell);
                }
                else
                {
                    Debug.Log("Bu h�creye yerle�tirme yap�lamaz: " + clickedCell);
                }
            }
            // T�klama sonras� di�er h�crelere dokunulmadan preview'lar korunur,
            // b�ylece timer s�f�rlanmaz.
        }
    }

    // �ste�e ba�l�: Yerle�tirme modunu sonland�r�p, kalan preview'lar� eski haline d�nd�rmek i�in
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
