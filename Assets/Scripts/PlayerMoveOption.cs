using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOption : MonoBehaviour
{
    public PlayerMovement master;

    public float walkAcceleration;
    public float walkDeceleration;
    public float walkSpeed;
    public float airAcceleration;

    public float jumpForce;

    Vector3 lastNormal = Vector3.zero;

    Vector3 horizontalSpeed
    {
        get { return Vector3.ProjectOnPlane(new Vector3(master.SPEED.x, 0, master.SPEED.z), lastNormal); }
        //get { return new Vector3(master.SPEED.x, 0, master.SPEED.z); }
    }

    public void Move()
    {
        if (!master.grounded) lastNormal = Vector3.up;

        if (master.grounded) GroundMovement();
        else AirMovement();
    }

    void GroundMovement()
    {
        Vector3 input = master.arrowInput;
        input = Vector3.ProjectOnPlane(input, lastNormal).normalized;
        input *= Time.deltaTime * walkAcceleration;
        input = Vector3.ClampMagnitude(input, walkSpeed - horizontalSpeed.magnitude);

        Fall();

        if ((horizontalSpeed + input).sqrMagnitude < walkSpeed * walkSpeed)
        {
            master.SPEED += input;
        }


        master.SPEED -= Vector3.ClampMagnitude(horizontalSpeed.normalized * Time.deltaTime * walkDeceleration, horizontalSpeed.magnitude);

        if (Input.GetKey(KeyCode.Space)) { master.SPEED.y = jumpForce; master.grounded = false; }
    }

    void AirMovement()
    {
        Vector3 input = master.arrowInput;
        input *= Time.deltaTime * airAcceleration;
        Vector3 result = horizontalSpeed + input;
        result = Vector3.ClampMagnitude(result, horizontalSpeed.magnitude);
        master.SPEED = result + Vector3.up * master.SPEED.y;

        Fall();
    }

    void Fall()
    {
        master.SPEED.y -= master.gravity * Time.deltaTime;
    }

    public void Collide(ControllerColliderHit hit)
    {
        lastNormal = hit.normal;

        Vector3 collisionForce = Vector3.Project(master.SPEED, hit.normal);
        master.SPEED -= collisionForce;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos, normal, force, result;
        startPos = new Vector3(0, 10, 0);
        force = new Vector3(0, 0, 3);
        normal = new Vector3(1, 1, 0);

        result = Vector3.ProjectOnPlane(force, normal).normalized;
        result *= force.magnitude;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(startPos, force);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(startPos, normal);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(startPos, result);
    }
}
