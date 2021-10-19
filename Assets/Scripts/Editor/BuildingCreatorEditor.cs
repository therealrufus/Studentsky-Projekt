using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingCreator))]
public class BuildingCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
		BuildingCreator generator = (BuildingCreator)target;

		if (GUILayout.Button("Generate"))
		{
			generator.Generate();
		}

		if (generator.autoUpdate) generator.Clamp();

		DrawDefaultInspector();
	}
}
