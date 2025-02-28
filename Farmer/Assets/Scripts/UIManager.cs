using UnityEngine;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{
    public TileChanger tileChanger; 

    [SerializeField] public TileBase Tile_Corn;
    [SerializeField] public TileBase Tile_Wheat;
    [SerializeField] public TileBase Tile_Carrot;

    public void ActivateTileChanger(GameObject button)
    {
        if (button.name.Contains("Wheat"))
        {
            tileChanger.finalTile = Tile_Wheat;
        }
        else if (button.name.Contains("Corn"))
        {
            tileChanger.finalTile = Tile_Corn;
        }
        else if (button.name.Contains("Carrot"))
        {
            tileChanger.finalTile = Tile_Carrot;
        }
        tileChanger.StartPlacing();
    }

}
