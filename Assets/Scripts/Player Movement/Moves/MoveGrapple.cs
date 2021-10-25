using UnityEngine;

public class MoveGrapple : PlayerMoveOption
{
    [Space(20)]
    [Tooltip("from where should the grapple be raycasted")]
    [SerializeField] Transform head;
    [Tooltip("how much should the player be affected by falling")]
    [Range(0f, 1f)]
    [SerializeField] float fallSpeedMultiplier;
    [Tooltip("at what looking angle should the player detach (-1: never, 1: always)")]
    [Range(-1f, 1f)]
    [SerializeField] float stopLookAngle = 0.25f;
    [Tooltip("the max reach of the grappling hook")]
    [SerializeField] float maxDistance = 999f;
    [Tooltip("how close of the grapple point should the player detach")]
    [SerializeField] float stopDistance = 5f;
    [Tooltip("the grappling acceleration")]
    [SerializeField] float acceleration = 50f;
    [Tooltip("the max grappling speed achieved by accelerating")]
    [SerializeField] float speedCap = 30f;
    [Tooltip("the initial boost toward the grapple point")]
    [SerializeField] float initialBoost = 10f;
    [Space]
    [Tooltip("how far can the player deviate from the grapple point")]
    [SerializeField] float maxAngle = 10f;


    [HideInInspector] public Vector3 impactPoint;
    Vector3 grappleInput;
    Vector3 anglePos;
    RaycastHit ray;

    Vector3 direction
    {
        get { return (impactPoint - transform.position).normalized; }
    }

    public override bool ShouldStart()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(head.position, head.forward, out ray, maxDistance))
            {
                return true;
            }
        }

        return false;
    }

    public override void Begin()
    {
        impactPoint = ray.point;
        //Impact(ray.normal * -1);
        grappleInput = Vector3.zero;
    }

    public override bool ShouldContinue()
    {
        if (Vector3.Distance(transform.position, impactPoint) <= stopDistance)
            return false;
        if (Vector3.Dot(head.forward, direction) < stopLookAngle)
            return false;
        if (Input.GetKey(KeyCode.LeftControl))
            return false;


        return true;
    }

    public override void Move()
    {
        base.Move();

        anglePos = Rotate();

        Vector3 force = (anglePos - transform.position).normalized;
        force = force * acceleration * Time.deltaTime;

        Vector3 speed = master.SPEED;
        speed += force;
        speed = Vector3.ClampMagnitude(speed, Mathf.Max(speedCap, master.SPEED.magnitude));
        master.SPEED = speed;

        Fall();

        Debug.DrawLine(transform.position, impactPoint, Color.black);
    }

    Vector3 Rotate()
    {
        Vector3 input = master.rawArrowInput;

        CameraInput();

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        grappleInput = Vector3.zero;

        grappleInput += Vector3.right * input.x;
        grappleInput += Vector3.up * input.z;
        grappleInput = grappleInput.normalized * maxAngle;

        return (rotation * grappleInput) + impactPoint;
    }

    Vector2 CameraInput()
    {
        Vector3 cameraDirection = head.forward;
        //Vector3 direction = (impactPoint - transform.position).normalized;
        
        return Vector3.zero;
    }

    void Impact(Vector3 normal)
    {
        if (Vector3.Dot(normal * -1, master.SPEED) > 0)
        {
            Vector3 collisionForce = Vector3.Project(master.SPEED, normal);
            master.SPEED -= collisionForce;
        }

        Vector3 force = direction;
        force *= initialBoost;

        master.SPEED += force;
    }

    protected override void Fall()
    {
        master.SPEED.y -= master.gravity * fallSpeedMultiplier * Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(impactPoint, 0.4f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(anglePos, 0.2f);
    }
}