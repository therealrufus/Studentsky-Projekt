using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKick : PlayerMoveOption
{
    [Space(20)]

    [SerializeField, Tooltip("layers to ignore when raycasting (player)")] LayerMask IgnoreMask;
    [Tooltip("max reach of the kick")] public float distance;
    [Tooltip("duration of the kick")] public float time;
    [Tooltip("delay of the kick")] public float delay;
    [SerializeField, Tooltip("minimal force to add to the player")] float minForce;
    [SerializeField, Tooltip("force added to the player (unnessesery)")] float addForce;

    RaycastHit ray;

    [HideInInspector] public float realDistance;
    bool shouldContinue;
    Vector3 direction;
    [HideInInspector] public float delayTime;

    public override bool ShouldStart()
    {
        if (Input.GetKeyDown(InputManager.AbSecondary))
        {
            return true;
        }

        return false;
    }

    public override void Begin()
    {
        shouldContinue = true;
        realDistance = 0f;
        delayTime = delay;
    }

    public override void Move()
    {
        base.Move();
        Fall();

        delayTime -= Time.deltaTime;
        if (delayTime > 0) return;

        realDistance += (distance / time) * Time.deltaTime;
        direction = master.head.forward;

        if (Physics.Raycast(master.head.position, master.head.forward, out ray, realDistance, ~IgnoreMask))
        {
            Hit();
        }

        if (realDistance >= distance) shouldContinue = false;
    }

    void Hit()
    {
        shouldContinue = false;

        float force = master.SPEED.magnitude + addForce;
        if (force < minForce) force = minForce;

        master.SPEED = direction * -1 * force;

        OnJump.Invoke();
    }

    public override bool ShouldContinue()
    {
        return shouldContinue;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(ray.point, 0.1f);
    }
}
