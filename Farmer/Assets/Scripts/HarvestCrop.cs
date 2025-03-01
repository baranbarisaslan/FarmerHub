using System;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestCrop : MonoBehaviour
{
    private TileChanger tileChanger;
    private Tilemap tilemap;
    private Vector3Int cropCell;

    void Start()
    {
        tileChanger = FindFirstObjectByType<TileChanger>();
        tilemap = tileChanger.tilemap;
        cropCell = tilemap.WorldToCell(transform.position);
    }
    void OnMouseDown()
    {
        Harvest();
    }

    void Harvest()
    {
        string cropname;
        string objname = gameObject.name.ToLower();
        if (objname.Contains("wheat"))
        {
            cropname = "Wheat";
        }
        else if (objname.Contains("carrot"))
        {
            cropname = "Carrot";
        }
        else
        {
            cropname = "Corn";
        }
        GameObject label = GameObject.Find(cropname);
        if (label != null)
        {
            string count = label.GetComponentInChildren<TextMeshProUGUI>().text;
            int countInt = Convert.ToInt32(count);
            countInt++;
            label.GetComponentInChildren<TextMeshProUGUI>().text = countInt.ToString();
        }
        tilemap.SetTile(cropCell, tileChanger.targetTile);
        tilemap.RefreshTile(cropCell);
    }
}
