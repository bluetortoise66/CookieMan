using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridManager))]
public class GridManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Show default inspector

        // "target" defines the current instance of the GridManager script
        // for every gameobject that has the GridManager script attached to it.
        // So, "target" is different for every gameobject
        GridManager gridManager = (GridManager)target;

        if (GUILayout.Button("Regenerate Grid"))
        {
            gridManager.RegenerateGrid();
            SceneView.RepaintAll(); // makes the changes appear immediately
        }

        if (GUILayout.Button("Clear Grid"))
        {
            gridManager.ToggleVisibility();
            SceneView.RepaintAll(); // makes the changes appear immediately
        }
    }
}