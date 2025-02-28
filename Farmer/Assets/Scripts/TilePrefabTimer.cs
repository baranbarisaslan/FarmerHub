using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePrefabTimer : MonoBehaviour
{
    [Header("Tile Ayarlarý")]
    // Tilemap, TileChanger üzerinden alýnacak
    private Tilemap tilemap;
    public TileBase targetTile;    // Örneðin: Sprite_Tiles_Elevation_6
    public TileBase matureTile;    // Delay sonunda konulacak tile
    public float delay = 15f;      // Bekleme süresi (15 saniye)

    // Bu prefab'ýn bulunduðu hücre
    private Vector3Int myCell;

    // Instance bazlý deðiþim kontrolü
    private HashSet<Vector3Int> changedCells = new HashSet<Vector3Int>();

    private void Start()
    {
        // Tilemap referansýný, TileChanger üzerinden alýyoruz
        TileChanger tileChanger = FindFirstObjectByType<TileChanger>();
        if (tileChanger != null)
        {
            tilemap = tileChanger.tilemap;
        }

        // Prefab'ýn bulunduðu world pozisyonundan, ilgili hücreyi hesapla
        myCell = tilemap.WorldToCell(transform.position);

        // Eðer bu hücre zaten deðiþtirildiyse, hiçbir iþlem yapmadan çýkýyoruz
        if (changedCells.Contains(myCell))
        {
            Debug.Log(gameObject.name + " - Bu hücre zaten deðiþtirildi: " + myCell);
            return;
        }

        StartCoroutine(ChangeTileAfterDelay());
    }

    private IEnumerator ChangeTileAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (!changedCells.Contains(myCell))
        {
            changedCells.Add(myCell); // Artýk bu hücre üzerinde iþlem yapýldý
            if (tilemap.GetTile(myCell) == targetTile)
            {
                TileBase newMatureTile = Instantiate(matureTile);
                tilemap.SetTile(myCell, newMatureTile);
                tilemap.RefreshTile(myCell);
            }

        }
    }
}