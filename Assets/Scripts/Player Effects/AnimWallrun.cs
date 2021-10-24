using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimWallrun : PlayerEffect
{
    [Space]
    public MoveWallride wallride;
    public Transform cam;
    Transform target;

    public float angle;
    public float time;

    float finalAngle;

    private void Start()
    {
        wallride.OnBegin.AddListener(RotateCam);
        wallride.OnEnd.AddListener(ResetCam);
        target = SpawnHolder(cam);
    }

    void RotateCam()
    {
        finalAngle = angle * AngleDir(wallride.transform.forward, wallride.normal, Vector3.up) * -1;

        target.DOLocalRotate(new Vector3(0, 0, finalAngle), time);
    }

    void ResetCam()
    {
        target.DOLocalRotate(new Vector3(0, 0, 0), time);
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
