using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBasic : PlayerMoveOption
{
    [Space]
    public float walkAcceleration = 100;
    public float walkDeceleration = 30;
    public float walkSpeed = 10;
    [Tooltip("the acceleration when pressing wasd in air")]public float airAcceleration = 20;
    [Tooltip("the max air speed achievable by wasd input only")]public float airSpeed = 2;
    public float jumpForce = 10;

    public override bool CheckState()
    {
        return true;
    }

    public override void Move()
    {
        base.Move();

        if (master.grounded) GroundMovement();
        else AirMovement();
    }

    void GroundMovement()
    {
        Fall();

        Vector3 input = master.arrowInput;
        input = Vector3.ProjectOnPlane(input, lastNormal).normalized;
        input *= Time.deltaTime * walkAcceleration;
        input = Vector3.ClampMagnitude(input, walkSpeed - horizontalSpeed.magnitude);

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

    private void OnDrawGizmos()
    {
        Vector3 startPos, normal, force, result;
        startPos = new Vector3(1, 10, 1);

        force = new Vector3(0.1f,1f, 0);
        normal = new Vector3(0, 1f, 0).normalized;

        result = Vector3.Project(force, normal);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(startPos - Vector3.right * 0.1f, force);

        Gizmos.color = Color.white;
        Gizmos.DrawRay(startPos, normal);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(startPos, result);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(startPos + Vector3.right * 0.1f, force - result);
    }
}
