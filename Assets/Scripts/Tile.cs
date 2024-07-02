using UnityEngine;

public class Tile : MonoBehaviour
{
    public Tile[] UpNeightbours;
    public Tile[] DownNeightbours;
    public Tile[] LeftNeightbours;
    public Tile[] RightNeightbours;

    public bool canBuildOnIt;
}
