using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI() {
        MapGenerator mg = (MapGenerator)target;

        if (DrawDefaultInspector()) {
            if (mg.autoUpdate)  {
                mg.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate")) {
            mg.DrawMapInEditor();
        }
    }
}
