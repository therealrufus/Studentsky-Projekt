using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimWallrun : MonoBehaviour
{
    public MoveWallride wallride;
    public Transform cam;
    Transform target;

    public float angle;
    public float time;

    public float finalAngle;

    private void Start()
    {
        wallride.OnBegin.AddListener(RotateCam);
        wallride.OnEnd.AddListener(ResetCam);
        target = SpawnHolder(cam);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) RotateCam();
    }

    void RotateCam()
    {
        finalAngle = angle * AngleDir(wallride.transform.forward, wallride.normal, Vector3.up) * -1;
        target.Rotate(new Vector3(0, 0, finalAngle));
        //target.DORotate(new Vector3(0, 0, angle), time, RotateMode.LocalAxisAdd);
    }

    void ResetCam()
    {
        target.Rotate(new Vector3(0, 0, -finalAngle));
        //target.DORotate(new Vector3(0, 0, -angle), time, RotateMode.LocalAxisAdd);
    }

    Transform SpawnHolder(Transform cam)
    {
        Transform target = new GameObject("holder").transform;
        target.parent = cam.parent;
        target.localPosition = cam.localPosition;
        target.localRotation = cam.localRotation;
        cam.parent = target;
        cam.localPosition = Vector3.zero;
        return target;
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return 1f;
        }
        else if (dir < 0f)
        {
            return -1f;
        }
        else
        {
            return 0f;
        }
    }
}
