using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCreator : MonoBehaviour
{
    public Vector3 SCALE;
    public bool autoUpdate;
    public float tileSize;
    Vector3 tileScale;

    public GameObject[] possibleTiles;
    Mesh[] meshes;
    Transform holder;




    public void Inicialize()
    {
        if (tileSize <= 0) tileSize = 1;
        if (holder != null) DestroyImmediate(holder.gameObject);

        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null) DestroyImmediate(filter);

        holder = new GameObject().transform;
        holder.parent = transform;

        holder.parent = null;
        holder.parent = transform;
    }
    public void Generate()
    {
        Inicialize();

        Quaternion startRotation = transform.rotation;
        Vector3 startPosition = transform.position;
        Vector3 startScale = transform.localScale;

        transform.rotation = Quaternion.identity;
        transform.position = Vector3.zero;
        transform.localScale = Vector3.one;

        if (tileSize <= 0) tileSize = 1;

        Clamp();

        tileScale = SCALE / tileSize;

        meshes = new Mesh[(int)(tileScale.x * tileScale.y * tileScale.z)];

        Vector3 offset = new Vector3(SCALE.x / 2, SCALE.y / 2, SCALE.z / 2);

        RotateSide(0, offset, Vector3.one * -1, new Vector2(tileScale.x, tileScale.y), 1);
        RotateSide(-90, offset, new Vector3(1, -1, -1), new Vector2(tileScale.z, tileScale.y), 2);
        RotateSide(-180, offset, new Vector3(1, -1, 1), new Vector2(tileScale.x, tileScale.y), 3);
        RotateSide(-270, offset, new Vector3(-1, -1, 1), new Vector2(tileScale.z, tileScale.y), 4);

        CombineMeshes();

        transform.rotation = startRotation;
        transform.position = startPosition;
        transform.localScale = startScale;
    }

    public void Clamp()
    {
        Vector3 scale = SCALE;
        scale = new Vector3(scale.x - (scale.x % tileSize), scale.y - (scale.y % tileSize), scale.z - (scale.z % tileSize));
        SCALE = scale;
    }

    void SpawnTiles(Vector2 size, int order)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Transform newTile = Instantiate(possibleTiles[Random.Range(0, possibleTiles.Length)], holder).transform;
                newTile.localPosition = new Vector3(x * tileSize, y * tileSize, 0);
                newTile.localPosition += new Vector3(tileSize / 2, tileSize / 2, 0);
                newTile.parent = transform;
            }
        }
    }

    void RotateSide(float rotation, Vector3 offset, Vector3 offsetMultiplier, Vector2 size, int order)
    {
        holder.localRotation = Quaternion.Euler(0, rotation, 0);
        holder.position = transform.position + Vector3.Scale(offset, offsetMultiplier);

        SpawnTiles(size, order);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, SCALE);
    }

    void CombineMeshes()
    {
        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshes.Length];
        int i = 0;
        while (i < meshes.Length)
        {
            combine[i].mesh = meshes[i].sharedMesh;
            combine[i].transform = meshes[i].transform.localToWorldMatrix;

            DestroyImmediate(meshes[i].gameObject);

            i++;
        }
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        meshFilter.sharedMesh.CombineMeshes(combine);
    }
}
