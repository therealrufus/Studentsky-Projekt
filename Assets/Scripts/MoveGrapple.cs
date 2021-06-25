using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class MoveGrapple : PlayerMoveOption
{
    public Transform head;
    [Range(-1f, 1f)]
    public float stopLoopAngle;
    public float maxDistance;
    public float stopDistance;
    public float speed;
    public float initialBoost;

    public float maxAngle;
    public Vector2 angularAcceleration;
    public float angularDeceleration;

    Vector3 impactPoint;
    Vector3 grappleInput;
    Vector3 anglePos;

    Vector3 direction
    {
        get { return (impactPoint - transform.position).normalized; }
    }

    public override bool CheckState()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RaycastHit ray;
            if (Physics.Raycast(head.position, head.forward, out ray, maxDistance))
            {
                impactPoint = ray.point;
                Impact(ray.normal * -1);
                grappleInput = Vector3.zero;
                return true;
            }
        }

        return false;
    }

    public override bool ShouldContinue()
    {
        if (Vector3.Distance(transform.position, impactPoint) <= stopDistance)
        {
            Debug.Log(Vector3.Distance(transform.position, impactPoint));
            return false;
        }

        if (Vector3.Dot(head.forward, direction) < stopLoopAngle)
            return false;

        if (Input.GetKey(KeyCode.LeftControl))
            return false;

        return true;
    }

    public override void Move()
    {
        base.Move();
        anglePos = Rotate();
        Vector3 force = (anglePos - transform.position).normalized;
        force = force * speed * Time.deltaTime;

        master.SPEED += force;

        Debug.DrawLine(transform.position, impactPoint, Color.black);
    }

    Vector3 Rotate()
    {
        Vector3 input = master.rawArrowInput;

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        grappleInput += Vector3.right * input.x * (angularAcceleration.x + angularDeceleration) * Time.deltaTime;
        grappleInput += Vector3.up * input.z * (angularAcceleration.y + angularDeceleration) * Time.deltaTime;

        grappleInput = Vector3.MoveTowards(grappleInput, Vector3.zero, angularDeceleration * Time.deltaTime);

        if (grappleInput.magnitude > maxAngle)
            grappleInput = grappleInput.normalized * maxAngle;

        return (rotation * grappleInput) + impactPoint;
    }

    void Impact(Vector3 normal)
    {
        if (Vector3.Dot(normal * -1, master.SPEED) > 0)
        {
            Vector3 collisionForce = Vector3.Project(master.SPEED, normal);
            master.SPEED -= collisionForce;
        }

        Vector3 force = direction;
        force *= initialBoost;

        master.SPEED += force;
    }

    private void OnDrawGizmos()
    {
        /*Vector3 sped = dir;
        Vector3 pos = new Vector3(4, 5, 1);
        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
        Vector3 result = Quaternion.AngleAxis(Time.time * 60, right) * sped;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(pos, sped);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, result);
        result = Quaternion.AngleAxis(Time.time * 60, Vector3.Cross(dir, right)) * sped;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(pos, result);*/

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(impactPoint, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(anglePos, 0.2f);
    }
}
