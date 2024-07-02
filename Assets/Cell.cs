using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isCollapsed;
    public Tile[] allPossibleTiles;

    public void CreateCell(bool collapsed, Tile[] tiles)
    {
        isCollapsed = collapsed;
        allPossibleTiles = tiles;
    }

    public void UpdateList(Tile[] tiles)
    {
        allPossibleTiles = tiles;
    }
}
