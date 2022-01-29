using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffector : PlayerEffector
{
    bool inside;
    Rigidbody rb;
    [Header("Float")]
    [SerializeField] LayerMask layer;
    [SerializeField] float hoverForce = 65f;
    [SerializeField] float hoverHeight = 3.5f;
    [Header("Move")]
    [SerializeField] float acceleration = 90f;
    [SerializeField] float maxSpeed = 200f;
    [SerializeField] float deceleration = 50f;
    [Header("Rotate")]
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float turnDeceleration = 50f;
    [SerializeField] float adjustRotationSpeed = 50f;

    float powerInput;
    float turnInput;

    Vector3 horizontalSpeed {
        get { return new Vector3(rb.velocity.x, 0, rb.velocity.z); }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        powerInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        Hover();
        MoveByInput();
        AdjustVelocity();
    }

    void MoveByInput()
    {
        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        rb.AddForce(forward * powerInput * acceleration);
        rb.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);
    }

    void Hover()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, hoverHeight, ~layer))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            rb.AddForce(appliedHoverForce, ForceMode.Force);
        }
    }

    void AdjustVelocity()
    {
        rb.AddForce(Vector3.ClampMagnitude(horizontalSpeed * -1 * deceleration, horizontalSpeed.magnitude), ForceMode.Acceleration);

        rb.AddRelativeTorque(Vector3.ClampMagnitude(rb.angularVelocity.normalized * -1 * turnDeceleration, rb.angularVelocity.magnitude));

        float horSpeedMagnitude = horizontalSpeed.magnitude;
        horSpeedMagnitude -= maxSpeed;
        if (horSpeedMagnitude > 0)
        {
            rb.AddForce(horizontalSpeed.normalized * horSpeedMagnitude * -1, ForceMode.VelocityChange);
        }

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 up = Vector3.up;
        Quaternion finalRotation = Quaternion.LookRotation(forward, up);
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, finalRotation, Time.deltaTime * 100 * adjustRotationSpeed);

        rb.MoveRotation(smoothRotation);
    }


    /*private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(InputManager.Action) && !inside)
        {

            if (GetPlayer(other.gameObject))
            {
                Subscribe();
                inside = true;
            }
        }
    }*/
}
