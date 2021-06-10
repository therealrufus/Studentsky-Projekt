using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOption : MonoBehaviour
{
    [HideInInspector]
    public PlayerMovement master;

    public int priority;

    protected Vector3 lastNormal = Vector3.zero;

    protected Vector3 horizontalSpeed
    {
        get { return Vector3.ProjectOnPlane(new Vector3(master.SPEED.x, 0, master.SPEED.z), lastNormal); }
    }

    public virtual void Move()
    {
        if (!master.grounded) lastNormal = Vector3.up;
    }

    protected virtual void Fall()
    {
        master.SPEED.y -= master.gravity * Time.deltaTime;
    }

    public virtual void Collide(ControllerColliderHit hit)
    {
        lastNormal = hit.normal;

        Vector3 collisionForce = Vector3.Project(master.SPEED, hit.normal);
        master.SPEED -= collisionForce;
    }

    public virtual bool CheckState()
    {
        return false;
    }
}
