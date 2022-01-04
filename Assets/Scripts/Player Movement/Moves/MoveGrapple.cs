using UnityEngine;

public class MoveGrapple : PlayerMoveOption
{
    [Space(20)]
    [SerializeField] float cooldown;
    [SerializeField] LayerMask IgnoreMask;
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
    [SerializeField, Tooltip("how much should each input axis modify the angle")] Vector2 inputMultiplier;


    [HideInInspector] public Vector3 impactPoint;
    Vector3 grappleInput;
    Vector3 anglePos;
    RaycastHit ray;
    float realCooldown = -1;

    public Vector3 direction
    {
        get { return (impactPoint - transform.position).normalized; }
    }

    private void Update()
    {
        realCooldown -= Time.deltaTime;
    }

    public override bool ShouldStart()
    {
        if (Input.GetKeyDown(InputManager.AbPrimary))
        {
            if (realCooldown <= 0)
            {
                if (Physics.Raycast(master.head.position, master.head.forward, out ray, maxDistance, ~IgnoreMask))
                {
                    return true;
                }
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
        {
            realCooldown = cooldown;
            return false;
        }
        if (Vector3.Dot(master.head.forward, direction) < stopLookAngle)
        {
            realCooldown = cooldown;
            return false;
        }
        if (Input.GetKey(InputManager.Crouch))
        {
            realCooldown = cooldown;
            return false;
        }


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
    }

    Vector3 Rotate()
    {
        Vector3 input = master.rawArrowInput;
        input = new Vector3(input.x * inputMultiplier.x, 0, CameraInput() * inputMultiplier.y);

        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);

        grappleInput = Vector3.zero;

        grappleInput += Vector3.right * input.x;
        grappleInput += Vector3.up * input.z;
        grappleInput = grappleInput* maxAngle;

        return (rotation * grappleInput) + impactPoint;
    }

    //not working
    float CameraInput()
    {
        Vector3 cameraDirection = master.head.forward;

        Vector3 vector = direction;
        vector.y = cameraDirection.y;

        float angle = (vector.normalized.y - direction.normalized.y);

        return angle;
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

        /*Vector3 offset = Vector3.up * 10;
        //Vector3 camDir = new Vector3(1, 1, 0.5f).normalized;
        //Vector3 dir = new Vector3(1, 0.5f, 0f).normalized;

        Vector3 h = dir;
        h.y = camDir.y;
        Gizmos.DrawRay(offset, camDir*10);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(offset, dir*10);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(offset, h*10);

        float angle = (h.normalized.y - dir.normalized.y) / 2;

        Debug.Log(angle);*/
    }
}