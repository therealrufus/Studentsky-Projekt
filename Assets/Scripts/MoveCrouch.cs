using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCrouch : PlayerMoveOption
{
    public float airAcceleration, airSpeed, jumpForce;
    public float Deceleration;
    public float airGravityMultiplier;
    public float groundGravityMultiplier;

    public override void Move()
    {

        base.Move();

        if (master.grounded) GroundMovement();
        else AirMovement();
    }

    void GroundMovement()
    {
        Fall();

        if (Input.GetKey(KeyCode.Space)) { master.SPEED.y = jumpForce; master.grounded = false; }

        master.SPEED -= Vector3.ClampMagnitude(horizontalSpeed.normalized * Time.deltaTime * Deceleration, horizontalSpeed.magnitude);
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

    protected override void Fall()
    {
        if (master.grounded)
            master.SPEED.y -= master.gravity * Time.deltaTime * groundGravityMultiplier;
        else master.SPEED.y -= master.gravity * Time.deltaTime * airGravityMultiplier;
    }
}
