using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimKick : PlayerEffect
{
    [SerializeField] MoveKick moveKick;
    Transform handLeft;
    Transform handRight;

    [SerializeField] float handPosAnticipation;

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

        if (moveKick.delayTime > 0)
        {
            float delayNormalized = 1f - (moveKick.delayTime / moveKick.delay);
            master.handLeft.localPosition = Vector3.forward * delayNormalized * handPosAnticipation * -1;
            master.handRight.localPosition = Vector3.forward * delayNormalized * handPosAnticipation * -1;
        }
        else
        {
            master.handLeft.localPosition = Vector3.forward * moveKick.realDistance;
            master.handRight.localPosition = Vector3.forward * moveKick.realDistance;
        }
    }
}
