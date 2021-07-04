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
        Vector3 point = Vector3.up;
        Vector3 dir = (point - transform.position).normalized;

        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
        Vector3 up = Vector3.Cross(dir, right);
        right = Vector3.right;
        up = Vector3.up;

        Vector3 result = Vector3.zero;

        result += up * Mathf.Sin(Time.time*5)*2;
        result += right * Mathf.Cos(Time.time * 5) * 2;
        result = Quaternion.LookRotation(dir, Vector3.up) * result;

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(point, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(result + point, 0.2f);
    }
}
