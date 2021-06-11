using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCrouch : PlayerMoveOption
{
    [Space]
    public MoveBasic basicMovement;

    public float airAccelerationMultiplier = 0.5f;
    public float Deceleration;
    public float airGravityMultiplier;
    public float groundGravityMultiplier;

    [Range(0, 1)]
    public float jumpDirectionalForce;

    public override bool CheckState()
    {
        return Input.GetKey(KeyCode.LeftControl);
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

        if (Input.GetKey(KeyCode.Space)) { Jump(); }

        master.SPEED -= Vector3.ClampMagnitude(horizontalSpeed.normalized * Time.deltaTime * Deceleration, horizontalSpeed.magnitude);
    }

    void AirMovement()
    {
        Vector3 input = master.arrowInput;
        input *= Time.deltaTime * basicMovement.airAcceleration * airAccelerationMultiplier;
        Vector3 result = horizontalSpeed + input;
        result = Vector3.ClampMagnitude(result, Mathf.Max(basicMovement.airSpeed, horizontalSpeed.magnitude));
        master.SPEED = result + Vector3.up * master.SPEED.y;

        Fall();
    }

    protected override void Fall()
    {
        if (master.grounded)
            master.SPEED.y -= master.gravity * Time.deltaTime * groundGravityMultiplier;
        else master.SPEED.y -= master.gravity * Time.deltaTime * airGravityMultiplier;
    }

    void Jump()
    {
        Vector3 speed = transform.forward;
        speed *= horizontalSpeed.magnitude;

        float dotProduct = 0.5f * Vector3.Dot(transform.forward, horizontalSpeed.normalized) + 0.5f;

        speed *= Mathf.Clamp(dotProduct+ jumpDirectionalForce, 0, 1);

        master.SPEED = speed + Vector3.up * basicMovement.jumpForce;
        master.grounded = false;
    }
}
