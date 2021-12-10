using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffector : PlayerEffector
{
    bool inside;
    Rigidbody rb;
    [SerializeField] LayerMask layer;
    [SerializeField] float speed = 90f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float hoverForce = 65f;
    [SerializeField] float hoverHeight = 3.5f;
    [SerializeField] float deceleration = 50f;

    float powerInput;
    float turnInput;

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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, hoverHeight, ~layer))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            rb.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        Vector3 forward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        rb.AddForce(forward * powerInput * speed);
        rb.AddForce(Vector3.ClampMagnitude(rb.velocity.normalized * -1 * deceleration, rb.velocity.magnitude));
        rb.AddRelativeTorque(0f, turnInput * turnSpeed, 0f);
    }


    private void OnTriggerStay(Collider other)
    {

        if (Input.GetKey(InputManager.Action) && !inside)
        {

            if (GetPlayer(other.gameObject))
            {
                Subscribe();
                inside = true;
            }
        }
    }
}
