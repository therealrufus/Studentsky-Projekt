using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] protected PlayerEffectsMaster master;

    protected Transform SpawnHolder(Transform cam)
    {
        Transform target = new GameObject("holder for " + GetType().Name).transform;
        target.parent = cam.parent;
        target.localPosition = cam.localPosition;
        target.localRotation = cam.localRotation;
        cam.parent = target;
        cam.localPosition = Vector3.zero;
        return target;
    }
}
