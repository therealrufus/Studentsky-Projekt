using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBasic : PlayerMoveOption
{
    public float walkAcceleration = 100;
    public float walkDeceleration = 30;
    public float walkSpeed = 10;
    public float airAcceleration = 20;
    public float airSpeed = 2;
    public float jumpForce = 10;

    public override void Move()
    {
        base.Move();

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
        result = Vector3.ClampMagnitude(result, Mathf.Max(airSpeed, horizontalSpeed.magnitude));
        master.SPEED = result + Vector3.up * master.SPEED.y;

        Fall();
    }

    /*private void OnDrawGizmos()
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
    }*/
}
