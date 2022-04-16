using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    public void Generate()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (mesh.GetTopology(0) != MeshTopology.Quads) 
        {
            Debug.LogError("not quads");
            return;
        }
        int[] indices = mesh.GetIndices(0);
        for (int i = 0; i < indices.Length/4; i++)
        {
            Vector3 v1 = mesh.vertices[indices[i]];
            Vector3 v2 = mesh.vertices[indices[i]+1];
            Vector3 v3 = mesh.vertices[indices[i]+2];
            Vector3 v4 = mesh.vertices[indices[i]+3];
            Vector3 center = v1 * 0.25f + v2 * 0.25f + v3 * 0.25f + v4 * 0.25f;
            Debug.DrawRay(center, Vector3.up, Color.red, 1);
        }
    }
}
