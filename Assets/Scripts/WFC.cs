using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


public class WFC : MonoBehaviour
{
    public bool activated;

    public GameObject building;

    public int width;   //size of the grid on x
    public int height;  //size of the grid on z

    public Cell cellPrefab; //Single cell prefab
    public Tile[] allTiles; //Array of all possible tiles

    public List<Cell> gridOfCells = new();  // List of all the cells in the grid
    private int iteration = 0;

    private void Start()
    {
        if (!activated)
        {
            Debug.LogWarning("WFC is not activated");
            return;
        }
        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(x, 0, z), Quaternion.identity);
                cell.CreateCell(false, allTiles);
                gridOfCells.Add(cell);
            }
        }

        StartCoroutine(Entropy());
    }

    IEnumerator Entropy()
    {
        //Get a copy of the original grid
        List<Cell> tempGrid = new List<Cell>(gridOfCells);
        //Remove the collapsed cells
        tempGrid.RemoveAll(cell => cell.isCollapsed);
        //Sort the list ascended
        tempGrid = tempGrid.OrderBy(cell => cell.allPossibleTiles.Length).ToList();

        //Sorting test
        //List<int> e = new List<int>() { 3, 5, 1, 4 ,9 ,6 };
        //e = e.OrderBy(a => a).ToList();
        //for (int i = 0; i < e.Count; i++)
        //{
        //    Debug.Log("pos tiles lenght " + e[i]);
        //}

        int arrLength = tempGrid[0].allPossibleTiles.Length;

        //// Find the index of the first cell with more tile options than the initial arrLength
        int stopIndex = tempGrid.FindIndex(cell => cell.allPossibleTiles.Length > arrLength);

        // Truncate the grid to cells with lowest options
        if (stopIndex >= 0)
        {
            tempGrid.RemoveRange(stopIndex, tempGrid.Count - stopIndex);
        }

        yield return new WaitForSeconds(0.01f);

        Collapse(tempGrid);
    }

    void Collapse(List<Cell> tempGrid)
    {
        //Get a random position on the grid
        Cell cell = tempGrid[Random.Range(0, tempGrid.Count)];
        cell.isCollapsed = true;

        Tile tile = cell.allPossibleTiles[Random.Range(0, cell.allPossibleTiles.Length)];

        //Make sure cell's possible tiles is the current tile so it doesnt change later
        cell.allPossibleTiles = new Tile[] { tile };

        Tile tilee = cell.allPossibleTiles[0];

        Tile go = Instantiate(tilee, cell.transform.position, tile.transform.rotation);
        //if (go.canBuildOnIt)
        //    PopulateBuildings(cell.transform.position);
        
        CheckNeighbours();
    }

    private void CheckNeighbours()
    {
        //copy the grid
        List<Cell> updatedCells = new List<Cell>(gridOfCells);

        //loop through the whole grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int currentCellIndex = y * width + x;

                //get the possible tiles
                List<Tile> possibleTiles = new List<Tile>(allTiles.ToList());

                if (gridOfCells[currentCellIndex].isCollapsed)
                    updatedCells[currentCellIndex] = gridOfCells[currentCellIndex];
                else
                {
                    //Check up
                    if (y > 0)
                    {
                        int index = x + (y - 1) * height;
                        Cell cell = gridOfCells[index];

                        List<Tile> validTiles = new();
                        foreach (Tile possibleTile in cell.allPossibleTiles)
                        {
                            for (int i = 0; i < allTiles.Length; i++)
                            {
                                if (possibleTile == allTiles[i])
                                {
                                    var valid = allTiles[i].UpNeightbours;
                                    validTiles.AddRange(valid);
                                }
                            }
                        }
                        CheckValidTiles(possibleTiles, validTiles);
                        Debug.Log("1st " + possibleTiles.Count);
                    }
                    //down
                    if (y < height - 1)
                    {
                        //index of the cell above
                        int upCellIndex = x + (y + 1) * height;
                        Cell cell = gridOfCells[upCellIndex];

                        //get current cell's tile's neighbours
                        //check which tiles are the same
                        //add it to a list
                        //check for valid tiles
                        List<Tile> validTiles = new();
                        foreach (Tile possibleTile in cell.allPossibleTiles)
                        {
                            for (int i = 0; i < allTiles.Length; i++)
                            {
                                if (possibleTile == allTiles[i])
                                {
                                    var valid = allTiles[i].DownNeightbours;
                                    validTiles.AddRange(valid);
                                }
                            }
                        }
                        CheckValidTiles(possibleTiles, validTiles);
                        Debug.Log("2st " + possibleTiles.Count);

                    }
                    //left neighbour
                    if (x < width - 1)
                    {
                        //index of the cell above
                        int upCellIndex = x + 1 + y * width;
                        Cell cell = gridOfCells[upCellIndex];

                        List<Tile> validTiles = new();
                        foreach (Tile possibleTile in cell.allPossibleTiles)
                        {
                            for (int i = 0; i < allTiles.Length; i++)
                            {
                                if (possibleTile == allTiles[i])
                                {
                                    var valid = allTiles[i].LeftNeightbours;
                                    validTiles.AddRange(valid);
                                }
                            }
                        }
                        CheckValidTiles(possibleTiles, validTiles);
                    }
                    //right neighbour
                    if (x > 0)
                    {
                        int upCellIndex = x - 1 + y * width;
                        Cell cell = gridOfCells[upCellIndex];

                        List<Tile> validTiles = new();
                        foreach (Tile possibleTile in cell.allPossibleTiles)
                        {
                            for (int i = 0; i < allTiles.Length; i++)
                            {
                                if (possibleTile == allTiles[i])
                                {
                                    var valid = allTiles[i].RightNeightbours;
                                    validTiles.AddRange(valid);
                                }
                            }
                        }
                        CheckValidTiles(possibleTiles, validTiles);
                    }

                    updatedCells[currentCellIndex].UpdateList(possibleTiles.ToArray());
                }

            }
        }
        gridOfCells = updatedCells;
        iteration++;
        if (iteration < width * height)
        {
            StartCoroutine(Entropy());
        }
        else
        {
            foreach (Cell cell in gridOfCells)
            {
                var tile = cell.allPossibleTiles[0];
                if (tile.canBuildOnIt)
                {
                    PopulateBuildings(cell.transform.position);
                }
            }
        }
    }

    private void PopulateBuildings(Vector3 position)
    {
        Instantiate(building, position, Quaternion.identity);
    }

    void CheckValidTiles(List<Tile> potentialTiles, List<Tile> validTiles)
    {
        for (int i = potentialTiles.Count - 1; i >= 0; i--)
        {
            if (!validTiles.Contains(potentialTiles[i]))
            {
                potentialTiles.RemoveAt(i);
            }
        }
    }
}