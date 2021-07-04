using UnityEngine;

public class MoveWallride : PlayerMoveOption
{
    [Space]
    public MoveBasic basicMovement;

    [Tooltip("the rate at witch the player should be decending on a wall")]public float gravity;
    [Tooltip("how fast should the speed be dropping")] public float deceleration;
    [Tooltip("how much should the player stick to a wall")] public float stickToSurfaceForce;
    [Range(0, 1)]
    [Tooltip("where should the player be looking to stick to a wall")] public float maxAngle;
    [Range(0, 1)]
    [Tooltip("how much can the wall be bended")] public float maxAngleUp;
    [Tooltip("the max distance to stick to a wall")] public float maxDistance;
    [Header("Jumping")]
    public float jumpForceUp;
    public float jumpForceSide;
    public float jumpForceForward;

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
            master.SPEED -= Vector3.up * -1 * deceleration * Time.deltaTime;
        }
        else base.Fall();
    }

    void Jump()
    {
        master.SPEED.y = jumpForceUp;
        master.SPEED += normal * jumpForceSide;
        master.SPEED += horizontalSpeed.normalized * jumpForceForward;
    }
}
