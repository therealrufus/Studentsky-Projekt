using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherBuildingCreator : MonoBehaviour
{
    public Vector2 positionRandomness;
    public Vector2 rotationRandomness;
    public Vector2 scaleRandomness;
    public Vector2Int floors;
    public Vector2 floorDescaler;

    public Building[] possibleObjects;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn(GetComponent<Collider>(), Random.Range(floors.x, floors.y), 1, 1);
        }
    }

    void Spawn(Collider parent, int stop, float scaleMultiplier, float positionY)
    {
        Building buildingPreset = possibleObjects[Random.Range(0, possibleObjects.Length)];
        GameObject obj = Instantiate(buildingPreset.obj);
        Collider col = obj.GetComponent<Collider>();

        int i = 0;
        do
        {
            Vector3 randomPos = new Vector3(
                Random.Range(positionRandomness.x, positionRandomness.y),
                Random.Range(positionRandomness.x, positionRandomness.y),
                Random.Range(positionRandomness.x, positionRandomness.y));
            obj.transform.position = parent.transform.position + randomPos * scaleMultiplier + Vector3.up * positionY;
            obj.transform.rotation = Quaternion.Euler(parent.transform.rotation.eulerAngles + new Vector3(0, Random.Range(rotationRandomness.x, rotationRandomness.y), 0));
            obj.transform.localScale = new Vector3(
                Random.Range(scaleRandomness.x, scaleRandomness.y),
                Random.Range(scaleRandomness.x, scaleRandomness.y),
                Random.Range(scaleRandomness.x, scaleRandomness.y)) * scaleMultiplier;
            i++;
            if (i > 50)
            {
                Debug.LogError("did not collide");
                break;
            }
        } while (CheckCollision(col));

        stop--;
        if (stop > 0) Spawn(col, stop, scaleMultiplier * Random.Range(floorDescaler.x, floorDescaler.y), buildingPreset.height * obj.transform.localScale.y);
    }

    bool CheckCollision(Collider col)
    {
        col.enabled = false;
        bool check = Physics.CheckBox(col.bounds.center, col.bounds.size / 2, col.transform.rotation);
        col.enabled = true;
        return check;
    }
}

[System.Serializable]
public class Building
{
    public GameObject obj;
    public float height = 1f;
}
