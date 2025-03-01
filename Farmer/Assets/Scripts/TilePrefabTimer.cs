using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilePrefabTimer : MonoBehaviour
{
    private Tilemap tilemap;
    public TileBase targetTile;    // Sprite_Tiles_Elevation_6
    public TileBase matureTile;   
    public float delay = 15f;      

    private Vector3Int myCell;

    private HashSet<Vector3Int> changedCells = new HashSet<Vector3Int>();

    private void Start()
    {
        TileChanger tileChanger = FindFirstObjectByType<TileChanger>();
        if (tileChanger != null)
        {
            tilemap = tileChanger.tilemap;
        }

        myCell = tilemap.WorldToCell(transform.position);

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
            changedCells.Add(myCell);
            if (tilemap.GetTile(myCell) == targetTile)
            {
                TileBase newMatureTile = Instantiate(matureTile);
                tilemap.SetTile(myCell, newMatureTile);
                tilemap.RefreshTile(myCell);
            }

        }
    }
}