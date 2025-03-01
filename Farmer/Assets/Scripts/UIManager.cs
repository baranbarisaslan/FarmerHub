using UnityEngine;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{
    public TileChanger tileChanger; 

    [SerializeField] public TileBase Tile_Corn;
    [SerializeField] public TileBase Tile_Wheat;
    [SerializeField] public TileBase Tile_Carrot;
    [SerializeField] public TileBase Tile_House;
    [SerializeField] public TileBase Tile_Tower;


    public void ActivateTileChanger(GameObject button)
    {
        if(tileChanger.isPlacing)
        {
            tileChanger.EndPlacing();
        }
        else
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
            else if (button.name.Contains("House"))
            {
                tileChanger.finalTile = Tile_House;
            }
            else if (button.name.Contains("Tower"))
            {
                tileChanger.finalTile = Tile_Tower;
            }
            tileChanger.StartPlacing();
        }
       
    }

}
