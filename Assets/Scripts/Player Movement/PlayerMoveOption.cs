using UnityEngine;
using UnityEngine.Events;

public class PlayerMoveOption : MonoBehaviour
{
    [HideInInspector]
    public PlayerMovement master;
    [Tooltip("the priority of this move in comparison of others")]
    public int priority;
    [Tooltip("is this move supposed to make something across a time?")]
    public bool hasDuration = false;

    //used for effects/animations for the player
    [HideInInspector]
    public UnityEvent OnMove;
    [HideInInspector]
    public UnityEvent OnBegin;
    [HideInInspector]
    public UnityEvent OnEnd;
    [HideInInspector]
    public UnityEvent OnJump;
    [HideInInspector]
    public UnityEvent OnLand;

    protected Vector3 lastNormal = Vector3.zero;

    protected Vector3 horizontalSpeed
    {
        get { return Vector3.ProjectOnPlane(new Vector3(master.SPEED.x, 0, master.SPEED.z), lastNormal); }
        //get { return new Vector3(master.SPEED.x, 0, master.SPEED.z); }
    }

    public virtual void Inicialize()
    { 
    
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

        Vector3 collisionForce = Vector3.zero;
        if (Vector3.Dot(lastNormal * -1, master.SPEED) > 0)
        {
            collisionForce = Vector3.Project(master.SPEED, lastNormal);
            master.SPEED -= collisionForce;

            //fuj
            if (master.grounded && master.groundedForFrames <= 1 && collisionForce.magnitude > 25f) OnLand.Invoke();
        }
    }

    public virtual bool ShouldStart()
    {
        return false;
    }

    public virtual bool ShouldContinue()
    {
        return false;
    }

    public virtual void Begin()
    {

    }

    public override string ToString()
    {
        return GetType().Name;
    }
}
