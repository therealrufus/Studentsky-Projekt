using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnimCrouch : PlayerEffect
{
    [SerializeField] PlayerMoveOption crouchMove;
    [SerializeField] Vector3 crouchedHeadPos;
    [SerializeField] float duration;

    Transform target;


    void Start()
    {
        crouchMove.OnBegin.AddListener(Crouch);
        crouchMove.OnEnd.AddListener(UnCrouch);

        target = SpawnHolder(master.camHolder.transform);
    }

    void Crouch()
    {
        target.DOLocalMove(crouchedHeadPos, duration);
    }

    void UnCrouch()
    {
        target.DOLocalMove(Vector3.zero, duration);
    }
}
