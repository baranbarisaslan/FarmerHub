using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePrefabTimer : MonoBehaviour
{
    [Header("Tile Ayarlar�")]
    // Tilemap, TileChanger �zerinden al�nacak
    private Tilemap tilemap;
    public TileBase targetTile;    // �rne�in: Sprite_Tiles_Elevation_6
    public TileBase matureTile;    // Delay sonunda konulacak tile
    public float delay = 15f;      // Bekleme s�resi (15 saniye)

    // Bu prefab'�n bulundu�u h�cre
    private Vector3Int myCell;

    // Instance bazl� de�i�im kontrol�
    private HashSet<Vector3Int> changedCells = new HashSet<Vector3Int>();

    private void Start()
    {
        // Tilemap referans�n�, TileChanger �zerinden al�yoruz
        TileChanger tileChanger = FindFirstObjectByType<TileChanger>();
        if (tileChanger != null)
        {
            tilemap = tileChanger.tilemap;
        }

        // Prefab'�n bulundu�u world pozisyonundan, ilgili h�creyi hesapla
        myCell = tilemap.WorldToCell(transform.position);

        // E�er bu h�cre zaten de�i�tirildiyse, hi�bir i�lem yapmadan ��k�yoruz
        if (changedCells.Contains(myCell))
        {
            Debug.Log(gameObject.name + " - Bu h�cre zaten de�i�tirildi: " + myCell);
            return;
        }

        StartCoroutine(ChangeTileAfterDelay());
    }

    private IEnumerator ChangeTileAfterDelay()
    {
        yield return new WaitForSeconds(delay);

        if (!changedCells.Contains(myCell))
        {
            changedCells.Add(myCell); // Art�k bu h�cre �zerinde i�lem yap�ld�
            if (tilemap.GetTile(myCell) == targetTile)
            {
                TileBase newMatureTile = Instantiate(matureTile);
                tilemap.SetTile(myCell, newMatureTile);
                tilemap.RefreshTile(myCell);
            }

        }
    }
}