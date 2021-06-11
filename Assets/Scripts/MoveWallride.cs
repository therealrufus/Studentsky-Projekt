using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallride : PlayerMoveOption
{
    [Space]
    public MoveBasic basicMovement;

    public float gravity;
    public float Deceleration;
    public float stickToSurfaceForce;
    [Range(0, 1)]
    public float maxAngle;
    [Range(0, 1)]
    public float maxAngleUp;
    public float maxDistance;
    [Header("Jumping")]
    public float jumpForceUp;
    public float jumpForceSide;

    Vector3 normal;


    public override bool CheckState()
    {
        if (master.grounded) return false;

        RaycastHit rayRight;
        RaycastHit rayLeft;

        if (Physics.Raycast(transform.position, transform.right, out rayRight, maxDistance))
        {
            if (Vector3.Dot(transform.right, rayRight.normal * -1) > maxAngle && Mathf.Abs(rayRight.normal.y) <= maxAngleUp)
            {
                normal = rayRight.normal;
                return true;
            }
        }
        else if (Physics.Raycast(transform.position, -transform.right, out rayLeft, maxDistance))
        {
            if (Vector3.Dot(-transform.right, rayLeft.normal * -1) > maxAngle && Mathf.Abs(rayLeft.normal.y) <= maxAngleUp)
            {
                normal = rayLeft.normal;
                return true;
            }
        }

        return false;
    }

    public override void Move()
    {
        base.Move();

        Vector3 input = master.arrowInput;
        input *= Time.deltaTime * basicMovement.airAcceleration;
        Vector3 result = horizontalSpeed + input;
        result = Vector3.ClampMagnitude(result, Mathf.Max(basicMovement.airSpeed, horizontalSpeed.magnitude));
        master.SPEED = result + Vector3.up * master.SPEED.y;

        master.SPEED -= normal * stickToSurfaceForce * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        Fall();
    }

    protected override void Fall()
    {
        if (master.SPEED.y < -1 * gravity)
        {
            master.SPEED -= Vector3.up * -1 * Deceleration * Time.deltaTime;
        }
        else base.Fall();
    }

    void Jump()
    {
        master.SPEED.y = jumpForceUp;
        master.SPEED += normal * jumpForceSide;
    }
}
