using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProceduralBuildingGeneration))]
public class EditorController : Editor
{
    public override void OnInspectorGUI()
    {
        ProceduralBuildingGeneration buildingGeneration = (ProceduralBuildingGeneration)target;

        if (DrawDefaultInspector()) {
            buildingGeneration.Build();
        }

        if (GUILayout.Button("Build"))
        {
            buildingGeneration.Build();
        } 

        if (GUILayout.Button("Clear"))
        {
            buildingGeneration.ClearBuildings();
        }
    }
}
