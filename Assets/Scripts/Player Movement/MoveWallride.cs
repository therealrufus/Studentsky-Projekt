using UnityEngine;
using UnityEngine.Events;

public class MoveWallride : PlayerMoveOption
{
    [Space(20)]
    [SerializeField] MoveBasic basicMovement;

    

    [Tooltip("the rate at witch the player should be decending on a wall")] public float gravity = 2f;
    [Tooltip("how fast should the speed be dropping")] public float deceleration = 10f;
    [Tooltip("how much should the player stick to a wall")] public float stickToSurfaceForce = 10f;
    [Range(0, 1)]
    [Tooltip("where should the player be looking to stick to a wall")] public float maxAngle = 0.5f;
    [Range(0, 1)]
    [Tooltip("how much can the wall be bended")] public float maxAngleUp = 0.2f;
    [Tooltip("the max distance to stick to a wall")] public float maxDistance = 1f;
    [Header("Jumping")]
    [SerializeField] float jumpForceUp = 20;
    [SerializeField] float jumpForceSide = 15;
    [SerializeField] float jumpForceForward = 2;

    [Space]
    [Tooltip("increase this to reduce flickering")] public float minimumTreshold;
    [HideInInspector]
    public Vector3 normal;

    public override bool ShouldStart()
    {
        if (master.grounded || (master.groundedForFrames <= minimumTreshold)) return false;

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
