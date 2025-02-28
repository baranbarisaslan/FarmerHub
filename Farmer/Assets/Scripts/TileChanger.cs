using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    public Tilemap tilemap;                   // Tilemap_Farm, Inspector'da atayýn
    public TileBase targetTile;               // Deðiþtirilmesi gereken tile (örn. Sprite_Tiles_Elevation_6)

    [Header("Önizleme Tile'larý")]
    public TileBase allowedPreviewTile;       // Ýzin verilen alan için (ör. yeþil)
    public TileBase disallowedPreviewTile;    // Ýzin verilmeyen alan için (ör. kýrmýzý)

    [Header("Kalýcý Tile")]
    public TileBase finalTile;                // Týklama sonrasý kalýcý tile

    [Header("Yerleþtirme Modu")]
    public bool isPlacing = false;            // Yerleþtirme modu aktif mi?

    // Hücrelerin orijinal tile'larýný saklar
    private Dictionary<Vector3Int, TileBase> originalTiles = new Dictionary<Vector3Int, TileBase>();

    public void StartPlacing()
    {
        isPlacing = true;
        originalTiles.Clear();

        // Tilemap'in tüm hücrelerini tarýyoruz
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
                    // Sadece targetTile için allowed, diðerleri için disallowed önizleme uygula
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

        // Kullanýcý týkladýðýnda yalnýzca týklanan hücre kontrol edilsin
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0;
            Vector3Int clickedCell = tilemap.WorldToCell(mouseWorldPos);

            if (originalTiles.ContainsKey(clickedCell))
            {
                // Sadece orijinal tile targetTile ise kalýcý tile yerleþtirilsin
                if (originalTiles[clickedCell] == targetTile)
                {
                    tilemap.SetTile(clickedCell, finalTile);
                    tilemap.RefreshTile(clickedCell);
                    Debug.Log("Tile yerleþtirildi: " + clickedCell);
                    // Bu hücre artýk restorasyona dahil edilmeyecek
                    originalTiles.Remove(clickedCell);
                }
                else
                {
                    Debug.Log("Bu hücreye yerleþtirme yapýlamaz: " + clickedCell);
                }
            }
            // Týklama sonrasý diðer hücrelere dokunulmadan preview'lar korunur,
            // böylece timer sýfýrlanmaz.
        }
    }

    // Ýsteðe baðlý: Yerleþtirme modunu sonlandýrýp, kalan preview'larý eski haline döndürmek için
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
