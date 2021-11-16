using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimKick : PlayerEffect
{
    [SerializeField] MoveKick moveKick;
    Transform handLeft;
    Transform handRight;

    private void Awake()
    {
        moveKick.OnMove.AddListener(Move);
        handLeft = SpawnHolder(master.handLeft);
        handRight = SpawnHolder(master.handRight);
    }

    void Move()
    {
        handLeft.rotation = master.cam.transform.rotation;
        handRight.rotation = master.cam.transform.rotation;
        master.handLeft.localPosition = Vector3.forward * moveKick.realDistance;
        master.handRight.localPosition = Vector3.forward * moveKick.realDistance;
    }
}
