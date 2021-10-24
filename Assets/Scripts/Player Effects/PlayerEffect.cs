using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] protected PlayerEffectsMaster master;

    protected Transform SpawnHolder(Transform obj)
    {
        Transform target = new GameObject("holder for " + GetType().Name).transform;
        target.parent = obj.parent;
        target.localPosition = obj.localPosition;
        target.localRotation = obj.localRotation;
        obj.parent = target;
        obj.localPosition = Vector3.zero;
        return target;
    }
}
