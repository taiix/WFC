using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralBuildingGeneration : MonoBehaviour
{
    [SerializeField] private GameObject[] buildingTop;
    [SerializeField] private GameObject[] buildingMiddle;
    [SerializeField] private GameObject[] buildingBottom;

    [SerializeField] private int minHeight;
    [SerializeField] private int maxHeight;

    [SerializeField] private List<GameObject> buildObjects = new();

    private void Start()
    {
        //Create the bottom part first
        StartCoroutine(Build());
    }

    public IEnumerator Build()
    {
        // Clear existing buildings
        ClearBuildings();

        // Instantiate foundation
        int randBottom = Random.Range(0, buildingBottom.Length);
        GameObject foundation = Instantiate(buildingBottom[randBottom], transform.position, Quaternion.identity, transform);
        buildObjects.Add(foundation);

        Vector3 foundationPosition = foundation.transform.position;

        // Determine building height
        int randHeight = Random.Range(minHeight, maxHeight);

        // Instantiate middle parts
        for (int i = 1; i < randHeight; i++)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject midPrefab = buildingMiddle[Random.Range(0, buildingMiddle.Length)];
            Vector3 midPos = buildObjects[buildObjects.Count - 1].transform.localPosition + Vector3.up * GetLastPartHeight();
            
            GameObject mid = Instantiate(midPrefab, new Vector3(foundationPosition.x, midPos.y, foundationPosition.z), Quaternion.identity, transform);
            buildObjects.Add(mid);
        }
        yield return new WaitForSeconds(0.2f);

        // Instantiate top part
        int randTop = Random.Range(0, buildingTop.Length);
        GameObject topPrefab = buildingTop[randTop];
        Vector3 topPos = buildObjects[buildObjects.Count - 1].transform.localPosition + Vector3.up * GetLastPartHeight();
        GameObject top = Instantiate(topPrefab, new Vector3(foundationPosition.x, topPos.y, foundationPosition.z), Quaternion.identity, transform);
        buildObjects.Add(top);
    }

    private float GetLastPartHeight()
    {
        MeshFilter mesh = buildObjects[buildObjects.Count - 1].GetComponent<MeshFilter>();
        Bounds bounds = mesh.sharedMesh.bounds;
        return bounds.size.y;
    }

    public void ClearBuildings()
    {
        Debug.Log("Clearing buildings...");
        foreach (GameObject obj in buildObjects)
        {
            DestroyImmediate(obj);
        }
        buildObjects.Clear();
        Debug.Log("Buildings cleared.");
    }
}
