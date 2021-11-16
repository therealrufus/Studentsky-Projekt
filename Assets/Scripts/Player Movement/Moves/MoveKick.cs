using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveKick : PlayerMoveOption
{
    [Space(20)]

    [SerializeField] LayerMask IgnoreMask;
    public float distance;
    public float time;
    [SerializeField] float minForce;
    [SerializeField] float addForce;

    RaycastHit ray;

    [HideInInspector] public float realDistance;
    bool shouldContinue;
    Vector3 direction;

    public override bool ShouldStart()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            return true;
        }

        return false;
    }

    public override void Begin()
    {
        shouldContinue = true;
        realDistance = 0f;
    }

    public override void Move()
    {
        base.Move();

        Fall();

        realDistance += (distance / time) * Time.deltaTime;
        direction = master.head.forward;

        if (Physics.Raycast(master.head.position, master.head.forward, out ray, realDistance, ~IgnoreMask))
        {
            shouldContinue = false;

            float force = master.SPEED.magnitude + addForce;
            if (force < minForce) force = minForce;

            master.SPEED = direction * -1 * force;
        }

        if (realDistance >= distance) shouldContinue = false;
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
