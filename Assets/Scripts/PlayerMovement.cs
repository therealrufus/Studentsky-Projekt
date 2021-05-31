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

    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public Vector3 SPEED;
    [HideInInspector]
    public Vector3 horizontalSpeed;
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

    void Update()
    {
        arrowInput = GetMovementInput();
        horizontalSpeed = new Vector3(SPEED.x, 0, SPEED.z);
        movement.Move();
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

        controller.Move(SPEED * Time.deltaTime);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        grounded = 1 - Vector3.Dot(hit.normal, Vector3.up) <= groundedAngle;
        movement.Collide(hit);
    }
}
