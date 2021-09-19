using UnityEngine;

public class MoveGrapple : PlayerMoveOption
{
    [Space(20)]
    [Tooltip("from where should the grapple be raycasted")]
    public Transform head;
    [Tooltip("at what looking angle should the player detach (-1: never, 1: always)")]
    [Range(-1f, 1f)]
    public float stopLookAngle = 0.25f;
    [Tooltip("the max reach of the grappling hook")]
    public float maxDistance = 999f;
    [Tooltip("how close of the grapple point should the player detach")]
    public float stopDistance = 5f;
    [Tooltip("the grappling acceleration")]
    public float acceleration = 50f;
    [Tooltip("the max grappling speed achieved by accelerating")]
    public float speedCap = 30f;
    [Tooltip("the initial boost toward the grapple point")]
    public float initialBoost = 10f;
    [Tooltip("the force to apply when jumping of a grapple move")]
    public float jumpForce = 20f;
    [Space]
    [Tooltip("how far can the player deviate from the grapple point")]
    public float maxAngle = 10f;

    Vector3 impactPoint;
    Vector3 grappleInput;
    Vector3 anglePos;
    RaycastHit ray;

    Vector3 direction
    {
        get { return (impactPoint - transform.position).normalized; }
    }

    public override bool ShouldStart()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Physics.Raycast(head.position, head.forward, out ray, maxDistance))
            {
                return true;
            }
        }

        return false;
    }

    public override void OnStart()
    {
        impactPoint = ray.point;
        Impact(ray.normal * -1);
        grappleInput = Vector3.zero;
    }

    public override bool ShouldContinue()
    {
        if (Vector3.Distance(transform.position, impactPoint) <= stopDistance)
        {
            return false;
        }

        if (Vector3.Dot(head.forward, direction) < stopLookAngle)
            return false;

        if (Input.GetKey(KeyCode.LeftControl))
            return false;

        if (Input.GetKey(KeyCode.Space))
        {
            if (master.SPEED.y > 0)
                master.SPEED += Vector3.up * jumpForce;
            else master.SPEED.y = jumpForce;
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
        Debug.Log(Vector3.Angle(cameraDirection, direction));
        
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

    private void OnDrawGizmos()
    {
        /*Vector3 sped = dir;
        Vector3 pos = new Vector3(4, 5, 1);
        Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
        Vector3 result = Quaternion.AngleAxis(Time.time * 60, right) * sped;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(pos, sped);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pos, result);
        result = Quaternion.AngleAxis(Time.time * 60, Vector3.Cross(dir, right)) * sped;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(pos, result);*/

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(impactPoint, 0.4f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(anglePos, 0.2f);
    }
}