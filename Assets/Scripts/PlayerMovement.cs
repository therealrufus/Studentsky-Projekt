using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Range(0, 1)]
    public float groundedAngle;
    public float gravity = 10f;
    public PlayerMoveOption movement;
    public PlayerMoveOption crouchMovement;

    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public Vector3 SPEED;
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public Vector3 arrowInput;

    [HideInInspector]
    public float planeSpeedSqrt
    {
        get { return new Vector2(SPEED.x, SPEED.z).sqrMagnitude; }
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {

    }

    void Update()
    {
        arrowInput = GetMovementInput();
        if (!Input.GetKey(KeyCode.LeftControl))
            movement.Move();
        else crouchMovement.Move();

        ApplyMovement();
    }

    Vector3 GetMovementInput()
    {
        Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        return (transform.right * rawInput.x + transform.forward * rawInput.z).normalized;
    }

    void ApplyMovement()
    {
        Debug.DrawRay(transform.position, SPEED, Color.red);

        CollisionFlags flags = controller.Move(SPEED * Time.deltaTime);
        if (flags == CollisionFlags.None) grounded = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        grounded = 1 - Vector3.Dot(hit.normal, Vector3.up) <= groundedAngle;
        if (!Input.GetKey(KeyCode.LeftControl))
            movement.Collide(hit);
        else crouchMovement.Collide(hit);
    }
}
