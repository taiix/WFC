//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using Unity.Collections;
//using Unity.Jobs;
//using Unity.Burst;
//using Unity.Mathematics;
//using Random = UnityEngine.Random;

//public class WaveFunctionCollapse : MonoBehaviour
//{
//    public int width;
//    public int height;

//    public GameObject prefab;
//    public TileData[] allTiles;
//    public NativeArray<CellData> cellData = new NativeArray<CellData>();
//    public NativeArray<float3> positions = new NativeArray<float3>();

//    public List<CellData> gridOfCells = new();  // List of all the cells in the grid

//    private void Start()
//    {
//        InitGrid();
//        Spawn();
//    }

//    void InitGrid()
//    {
//        cellData = new NativeArray<CellData>(width * height, Allocator.Persistent);
//        for (int y = 0; y < height; y++)
//        {
//            for (int x = 0; x < width; x++)
//            {
//                int index = y * width + x;
//                cellData[index] = new CellData(x, y, false, allTiles);
//                gridOfCells.Add(cellData[index]);
//            }
//        }
//        //StartCoroutine(Entropy());
//    }

//    //IEnumerator Entropy()
//    //{
//    //Get a copy of the original grid
//    //    List<CellData> tempGrid = new List<CellData>(gridOfCells);
//    //    //Remove the collapsed cells
//    //    tempGrid.RemoveAll(cell => cell.isCollapsed);
//    //    //Sort the list ascended
//    //   // tempGrid = tempGrid.OrderBy(cell => cell.allPossibleTiles.Length).ToList();

//    //    //Sorting test
//    //    //List<int> e = new List<int>() { 3, 5, 1, 4 ,9 ,6 };
//    //    //e = e.OrderBy(a => a).ToList();
//    //    //for (int i = 0; i < e.Count; i++)
//    //    //{
//    //    //    Debug.Log("pos tiles lenght " + e[i]);
//    //    //}

//    //    int arrLength = tempGrid[0].allPossibleTiles.Length;

//    //    //// Find the index of the first cell with more tile options than the initial arrLength
//    //    int stopIndex = tempGrid.FindIndex(cell => cell.allPossibleTiles.Length > arrLength);

//    //    // Truncate the grid to cells with lowest options
//    //    if (stopIndex >= 0)
//    //    {
//    //        tempGrid.RemoveRange(stopIndex, tempGrid.Count - stopIndex);
//    //    }

//    //    yield return new WaitForSeconds(0.01f);

//    //    Collapse(tempGrid);
//    //}

//    //void Collapse(List<CellData> tempGrid)
//    //{
//    //    //Get a random position on the grid
//    //    Cell cell = tempGrid[Random.Range(0, tempGrid.Count)];
//    //    cell.isCollapsed = true;

//    //    Tile tile = cell.allPossibleTiles[Random.Range(0, cell.allPossibleTiles.Length)];

//    //    //Make sure cell's possible tiles is the current tile so it doesnt change later
//    //    cell.allPossibleTiles = new Tile[] { tile };

//    //    Tile tilee = cell.allPossibleTiles[0];

//    //    Tile go = Instantiate(tilee, cell.transform.position, tile.transform.rotation);
//    //}

//    void Spawn()
//    {
//        positions = new NativeArray<float3>(width * height, Allocator.TempJob);

//        SpawnGridJob job = new SpawnGridJob()
//        {
//            width = width,
//            height = height,
//            cellData = cellData,
//            positions = positions,
//            allTiles = allTiles
//        };

//        JobHandle jobHandler = job.Schedule(cellData.Length, 64);
//        jobHandler.Complete();

//        for (int i = 0; i < positions.Length; i++)
//        {
//            Instantiate(prefab, positions[i], Quaternion.identity);
//        }

//        cellData.Dispose();
//        positions.Dispose();
//    }
//}

//public struct SpawnGridJob : IJobParallelFor
//{
//    public int width;
//    public int height;

//    [ReadOnly] public NativeArray<CellData> cellData;
//    [ReadOnly] public NativeArray<float3> positions;

//    [ReadOnly] public TileData[] allTiles;

//    void CheckValidTiles(List<Tile> potentialTiles, List<Tile> validTiles)
//    {
//        for (int i = potentialTiles.Count - 1; i >= 0; i--)
//        {
//            if (!validTiles.Contains(potentialTiles[i]))
//            {
//                potentialTiles.RemoveAt(i);
//            }
//        }
//    }

//    public void Execute(int index)
//    {
//        //CellData cell = cellData[index];

//        //float3 position = new float3(cell.x, 0, cell.y);
//        //positions[index] = position;

//        //    List<CellData> updatedCells = new List<CellData>(cellData);

//        //    //loop through the whole grid
//        //    for (int y = 0; y < height; y++)
//        //    {
//        //        for (int x = 0; x < width; x++)
//        //        {
//        //            int currentCellIndex = y * width + x;

//        //            //get the possible tiles
//        //            List<Tile> possibleTiles = new List<Tile>(allTiles.ToList());

//        //            if (cellData[currentCellIndex].isCollapsed)
//        //                updatedCells[currentCellIndex] = cellData[currentCellIndex];
//        //            else
//        //            {
//        //                //Check up
//        //                if (y > 0)
//        //                {
//        //                    int indexx = x + (y - 1) * height;
//        //                    CellData cell = cellData[indexx];

//        //                    List<Tile> validTiles = new();
//        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
//        //                    {
//        //                        for (int i = 0; i < allTiles.Length; i++)
//        //                        {
//        //                            if (possibleTile == allTiles[i])
//        //                            {
//        //                                var valid = allTiles[i].UpNeightbours;
//        //                                validTiles.AddRange(valid);
//        //                            }
//        //                        }
//        //                    }
//        //                    CheckValidTiles(possibleTiles, validTiles);
//        //                    Debug.Log("1st " + possibleTiles.Count);
//        //                }
//        //                //down
//        //                if (y < height - 1)
//        //                {
//        //                    //index of the cell above
//        //                    int upCellIndex = x + (y + 1) * height;
//        //                    CellData cell = cellData[upCellIndex];

//        //                    //get current cell's tile's neighbours
//        //                    //check which tiles are the same
//        //                    //add it to a list
//        //                    //check for valid tiles
//        //                    List<Tile> validTiles = new();
//        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
//        //                    {
//        //                        for (int i = 0; i < allTiles.Length; i++)
//        //                        {
//        //                            if (possibleTile == allTiles[i])
//        //                            {
//        //                                var valid = allTiles[i].DownNeightbours;
//        //                                validTiles.AddRange(valid);
//        //                            }
//        //                        }
//        //                    }
//        //                    CheckValidTiles(possibleTiles, validTiles);
//        //                    Debug.Log("2st " + possibleTiles.Count);

//        //                }
//        //                //left neighbour
//        //                if (x < width - 1)
//        //                {
//        //                    //index of the cell above
//        //                    int upCellIndex = x + 1 + y * width;
//        //                    CellData cell = cellData[upCellIndex];

//        //                    List<Tile> validTiles = new();
//        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
//        //                    {
//        //                        for (int i = 0; i < allTiles.Length; i++)
//        //                        {
//        //                            if (possibleTile == allTiles[i])
//        //                            {
//        //                                var valid = allTiles[i].LeftNeightbours;
//        //                                validTiles.AddRange(valid);
//        //                            }
//        //                        }
//        //                    }
//        //                    CheckValidTiles(possibleTiles, validTiles);
//        //                }
//        //                //right neighbour
//        //                if (x > 0)
//        //                {
//        //                    int upCellIndex = x - 1 + y * width;
//        //                    CellData cell = cellData[upCellIndex];

//        //                    List<Tile> validTiles = new();
//        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
//        //                    {
//        //                        for (int i = 0; i < allTiles.Length; i++)
//        //                        {
//        //                            if (possibleTile == allTiles[i])
//        //                            {
//        //                                var valid = allTiles[i].RightNeightbours;
//        //                                validTiles.AddRange(valid);
//        //                            }
//        //                        }
//        //                    }
//        //                    CheckValidTiles(possibleTiles, validTiles);
//        //                }

//        //                updatedCells[currentCellIndex].UpdateList(possibleTiles.ToArray());
//        //            }

//        //        }
//        //    }
//        //}
//    }
//}

//[System.Serializable]
//public struct CellData
//{
//    public int x;
//    public int y;

//    public bool isCollapsed;
//    [ReadOnly] public TileData[] allPossibleTiles;

//    public CellData(int x, int y, bool isCollapsed, [ReadOnly] TileData[] allPossibleTiles)
//    {
//        this.x = x;
//        this.y = y;
//        this.isCollapsed = isCollapsed;
//        this.allPossibleTiles = allPossibleTiles;
//    }

//    public void UpdateList(TileData[] tiles)
//    {
//        allPossibleTiles = tiles;
//    }
//}

//[System.Serializable]
//public struct TileData
//{
//    TileData[] up;
//    TileData[] down;
//    TileData[] left;
//    TileData[] right;
//}

////using System.Collections;
////using System.Collections.Generic;
////using System.Linq;
////using UnityEngine;
////using Unity.Collections;
////using Unity.Jobs;
////using Unity.Burst;
////using Unity.Mathematics;
////using Random = UnityEngine.Random;

////public class WaveFunctionCollapse : MonoBehaviour
////{
////    public int width;
////    public int height;

////    public GameObject prefab;
////    public TileData[] allTiles;
////    public NativeArray<CellData> cellData = new NativeArray<CellData>();
////    public NativeArray<float3> positions = new NativeArray<float3>();

////    public List<CellData> gridOfCells = new();  // List of all the cells in the grid

////    private void Start()
////    {
////        InitGrid();
////        Spawn();
////    }

////    void InitGrid()
////    {
////        cellData = new NativeArray<CellData>(width * height, Allocator.Persistent);
////        for (int y = 0; y < height; y++)
////        {
////            for (int x = 0; x < width; x++)
////            {
////                int index = y * width + x;
////                cellData[index] = new CellData(x, y, false, allTiles);
////                gridOfCells.Add(cellData[index]);
////            }
////        }
////        //StartCoroutine(Entropy());
////    }

////    //IEnumerator Entropy()
////    //{
////    //Get a copy of the original grid
////    //    List<CellData> tempGrid = new List<CellData>(gridOfCells);
////    //    //Remove the collapsed cells
////    //    tempGrid.RemoveAll(cell => cell.isCollapsed);
////    //    //Sort the list ascended
////    //   // tempGrid = tempGrid.OrderBy(cell => cell.allPossibleTiles.Length).ToList();

////    //    //Sorting test
////    //    //List<int> e = new List<int>() { 3, 5, 1, 4 ,9 ,6 };
////    //    //e = e.OrderBy(a => a).ToList();
////    //    //for (int i = 0; i < e.Count; i++)
////    //    //{
////    //    //    Debug.Log("pos tiles lenght " + e[i]);
////    //    //}

////    //    int arrLength = tempGrid[0].allPossibleTiles.Length;

////    //    //// Find the index of the first cell with more tile options than the initial arrLength
////    //    int stopIndex = tempGrid.FindIndex(cell => cell.allPossibleTiles.Length > arrLength);

////    //    // Truncate the grid to cells with lowest options
////    //    if (stopIndex >= 0)
////    //    {
////    //        tempGrid.RemoveRange(stopIndex, tempGrid.Count - stopIndex);
////    //    }

////    //    yield return new WaitForSeconds(0.01f);

////    //    Collapse(tempGrid);
////    //}

////    //void Collapse(List<CellData> tempGrid)
////    //{
////    //    //Get a random position on the grid
////    //    Cell cell = tempGrid[Random.Range(0, tempGrid.Count)];
////    //    cell.isCollapsed = true;

////    //    Tile tile = cell.allPossibleTiles[Random.Range(0, cell.allPossibleTiles.Length)];

////    //    //Make sure cell's possible tiles is the current tile so it doesnt change later
////    //    cell.allPossibleTiles = new Tile[] { tile };

////    //    Tile tilee = cell.allPossibleTiles[0];

////    //    Tile go = Instantiate(tilee, cell.transform.position, tile.transform.rotation);
////    //}

////    void Spawn()
////    {
////        positions = new NativeArray<float3>(width * height, Allocator.TempJob);

////        SpawnGridJob job = new SpawnGridJob()
////        {
////            width = width,
////            height = height,
////            cellData = cellData,
////            positions = positions,
////            allTiles = allTiles
////        };

////        JobHandle jobHandler = job.Schedule(cellData.Length, 64);
////        jobHandler.Complete();

////        for (int i = 0; i < positions.Length; i++)
////        {
////            Instantiate(prefab, positions[i], Quaternion.identity);
////        }

////        cellData.Dispose();
////        positions.Dispose();
////    }
////}

////public struct SpawnGridJob : IJobParallelFor
////{
////    public int width;
////    public int height;

////    [ReadOnly] public NativeArray<CellData> cellData;
////    [ReadOnly] public NativeArray<float3> positions;

////    [ReadOnly] public TileData[] allTiles;

////    void CheckValidTiles(List<Tile> potentialTiles, List<Tile> validTiles)
////    {
////        for (int i = potentialTiles.Count - 1; i >= 0; i--)
////        {
////            if (!validTiles.Contains(potentialTiles[i]))
////            {
////                potentialTiles.RemoveAt(i);
////            }
////        }
////    }

////    public void Execute(int index)
////    {
////        //CellData cell = cellData[index];

////        //float3 position = new float3(cell.x, 0, cell.y);
////        //positions[index] = position;

////        //    List<CellData> updatedCells = new List<CellData>(cellData);

////        //    //loop through the whole grid
////        //    for (int y = 0; y < height; y++)
////        //    {
////        //        for (int x = 0; x < width; x++)
////        //        {
////        //            int currentCellIndex = y * width + x;

////        //            //get the possible tiles
////        //            List<Tile> possibleTiles = new List<Tile>(allTiles.ToList());

////        //            if (cellData[currentCellIndex].isCollapsed)
////        //                updatedCells[currentCellIndex] = cellData[currentCellIndex];
////        //            else
////        //            {
////        //                //Check up
////        //                if (y > 0)
////        //                {
////        //                    int indexx = x + (y - 1) * height;
////        //                    CellData cell = cellData[indexx];

////        //                    List<Tile> validTiles = new();
////        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
////        //                    {
////        //                        for (int i = 0; i < allTiles.Length; i++)
////        //                        {
////        //                            if (possibleTile == allTiles[i])
////        //                            {
////        //                                var valid = allTiles[i].UpNeightbours;
////        //                                validTiles.AddRange(valid);
////        //                            }
////        //                        }
////        //                    }
////        //                    CheckValidTiles(possibleTiles, validTiles);
////        //                    Debug.Log("1st " + possibleTiles.Count);
////        //                }
////        //                //down
////        //                if (y < height - 1)
////        //                {
////        //                    //index of the cell above
////        //                    int upCellIndex = x + (y + 1) * height;
////        //                    CellData cell = cellData[upCellIndex];

////        //                    //get current cell's tile's neighbours
////        //                    //check which tiles are the same
////        //                    //add it to a list
////        //                    //check for valid tiles
////        //                    List<Tile> validTiles = new();
////        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
////        //                    {
////        //                        for (int i = 0; i < allTiles.Length; i++)
////        //                        {
////        //                            if (possibleTile == allTiles[i])
////        //                            {
////        //                                var valid = allTiles[i].DownNeightbours;
////        //                                validTiles.AddRange(valid);
////        //                            }
////        //                        }
////        //                    }
////        //                    CheckValidTiles(possibleTiles, validTiles);
////        //                    Debug.Log("2st " + possibleTiles.Count);

////        //                }
////        //                //left neighbour
////        //                if (x < width - 1)
////        //                {
////        //                    //index of the cell above
////        //                    int upCellIndex = x + 1 + y * width;
////        //                    CellData cell = cellData[upCellIndex];

////        //                    List<Tile> validTiles = new();
////        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
////        //                    {
////        //                        for (int i = 0; i < allTiles.Length; i++)
////        //                        {
////        //                            if (possibleTile == allTiles[i])
////        //                            {
////        //                                var valid = allTiles[i].LeftNeightbours;
////        //                                validTiles.AddRange(valid);
////        //                            }
////        //                        }
////        //                    }
////        //                    CheckValidTiles(possibleTiles, validTiles);
////        //                }
////        //                //right neighbour
////        //                if (x > 0)
////        //                {
////        //                    int upCellIndex = x - 1 + y * width;
////        //                    CellData cell = cellData[upCellIndex];

////        //                    List<Tile> validTiles = new();
////        //                    foreach (Tile possibleTile in cell.allPossibleTiles)
////        //                    {
////        //                        for (int i = 0; i < allTiles.Length; i++)
////        //                        {
////        //                            if (possibleTile == allTiles[i])
////        //                            {
////        //                                var valid = allTiles[i].RightNeightbours;
////        //                                validTiles.AddRange(valid);
////        //                            }
////        //                        }
////        //                    }
////        //                    CheckValidTiles(possibleTiles, validTiles);
////        //                }

////        //                updatedCells[currentCellIndex].UpdateList(possibleTiles.ToArray());
////        //            }

////        //        }
////        //    }
////        //}
////    }
////}

////[System.Serializable]
////public struct CellData
////{
////    public int x;
////    public int y;

////    public bool isCollapsed;
////    [ReadOnly] public TileData[] allPossibleTiles;

////    public CellData(int x, int y, bool isCollapsed, [ReadOnly] TileData[] allPossibleTiles)
////    {
////        this.x = x;
////        this.y = y;
////        this.isCollapsed = isCollapsed;
////        this.allPossibleTiles = allPossibleTiles;
////    }

////    public void UpdateList(TileData[] tiles)
////    {
////        allPossibleTiles = tiles;
////    }
////}

////[System.Serializable]
////public struct TileData
////{
////    TileData[] up;
////    TileData[] down;
////    TileData[] left;
////    TileData[] right;
////}

