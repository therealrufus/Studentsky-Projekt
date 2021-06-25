using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOption : MonoBehaviour
{
    [HideInInspector]
    public PlayerMovement master;
    [Tooltip("the priority of this move in comparison of others")]
    public int priority;
    [Tooltip("is this move supposed to make something across a time?")]
    public bool hasDuration = false;

    protected Vector3 lastNormal = Vector3.zero;

    protected Vector3 horizontalSpeed
    {
        get { return Vector3.ProjectOnPlane(new Vector3(master.SPEED.x, 0, master.SPEED.z), lastNormal); }
        //get { return new Vector3(master.SPEED.x, 0, master.SPEED.z); }
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

        if (Vector3.Dot(lastNormal * -1, master.SPEED) > 0)
        {
            Vector3 collisionForce = Vector3.Project(master.SPEED, lastNormal);
            master.SPEED -= collisionForce;
        }
    }

    public virtual bool CheckState()
    {
        return false;
    }

    public virtual bool ShouldContinue()
    {
        return true;
    }
}
