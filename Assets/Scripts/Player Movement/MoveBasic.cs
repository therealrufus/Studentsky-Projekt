using UnityEngine;

public class MoveBasic : PlayerMoveOption
{
    [Space(20)]
    public float walkAcceleration = 100;
    public float walkDeceleration = 30;
    public float walkSpeed = 10;
    [Space]
    [Tooltip("the acceleration when pressing wasd in air")]public float airAcceleration = 20;
    [Tooltip("the max air speed achievable by wasd input only")]public float airSpeed = 2;
    [Space]
    public float jumpForce = 10;

    public override bool ShouldStart()
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
}
