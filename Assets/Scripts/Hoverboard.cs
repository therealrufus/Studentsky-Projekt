using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hoverboard : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float hoverForce;
    [SerializeField] float deceleration;

    [SerializeField] Transform[] jets;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void LateUpdate()
    {
        foreach (Transform jet in jets)
        {
            ApplyForce(jet);
        }
    }

    void ApplyForce(Transform pos, bool useRotation = false)
    {
        RaycastHit hit;
        if (Physics.Raycast(pos.position, useRotation ? pos.up * -1 : Vector3.down, out hit))
        {
            float force = Mathf.Abs(1 / (hit.point.y - pos.position.y));
            rb.AddForceAtPosition(transform.up * force * hoverForce, pos.position, ForceMode.Acceleration);

        }
    }
}
