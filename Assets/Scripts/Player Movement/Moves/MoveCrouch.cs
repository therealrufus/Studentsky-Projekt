using UnityEngine;

public class MoveCrouch : PlayerMoveOption
{
    [Space(20)]
    [SerializeField] MoveBasic basicMovement;

    [Range(0, 1)]
    [Tooltip("How much is the player boosted on impact")]
    [SerializeField] float boostForce = 0.3f;
    [Tooltip("should the player strafe slower when crouching in air?")]
    [SerializeField] float airAccelerationMultiplier = 0.5f;
    [Tooltip("ground friction")]
    [SerializeField] float deceleration = 10f;
    [Space]
    [Tooltip("should the player be heavier when crouching?")]
    [SerializeField] float airGravityMultiplier = 1.5f;
    [Tooltip("bigger value, bigger acceleration on slopes")]
    [SerializeField] float groundGravityMultiplier = 4f;
    [Tooltip("bigger value, bigger acceleration on slopes")]
    [SerializeField] float groundClimbGravityMultiplier = 1.5f;

    [Range(0, 1)]
    [Tooltip("how much speed at least should be preserved when jumping")] public float jumpDirectionalForce = 0.1f;

    public override bool ShouldStart()
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

        master.SPEED -= Vector3.ClampMagnitude(horizontalSpeed.normalized * Time.deltaTime * deceleration, horizontalSpeed.magnitude);
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
        master.SPEED.y -= master.gravity * Time.deltaTime * GravityToApply();
    }

    void Jump()
    {
        Vector3 speed = transform.forward;
        speed *= horizontalSpeed.magnitude;

        float dotProduct = 0.5f * Vector3.Dot(transform.forward, horizontalSpeed.normalized) + 0.5f;

        speed *= Mathf.Clamp(dotProduct + jumpDirectionalForce, 0, 1);

        master.SPEED = speed + Vector3.up * basicMovement.jumpForce;
        master.grounded = false;

        OnJump.Invoke();
    }

    public override void Collide(ControllerColliderHit hit)
    {
        lastNormal = hit.normal;

        if (Vector3.Dot(lastNormal * -1, master.SPEED) > 0)
        {
            Vector3 collisionForce = Vector3.Project(master.SPEED, lastNormal);
            Vector3 optionOne = master.SPEED - collisionForce;
            Vector3 optionTwo = optionOne.normalized * master.SPEED.magnitude;
            //tohle bych asi mel okomentovat
            master.SPEED = Vector3.Lerp(optionOne, optionTwo, boostForce);
        }
    }

    float GravityToApply()
    {
        float force = airGravityMultiplier;

        if (master.grounded)
        {
            force = groundGravityMultiplier;

            Vector3 horizontalPlayerDir = master.SPEED;
            horizontalPlayerDir.y = 0;
            horizontalPlayerDir = horizontalPlayerDir.normalized;

            Vector3 groundDir = lastNormal;
            groundDir.y = 0;
            groundDir = groundDir.normalized;

            if (Vector3.Dot(horizontalPlayerDir, groundDir) < 0)
                force = groundClimbGravityMultiplier;
        }
        return force;
    }
}
