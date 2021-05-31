using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveOption : MonoBehaviour
{
    public PlayerMovement master;

    public float walkAcceleration;
    public float walkDeceleration;
    public float walkSpeed;

    public float jumpForce;

    public void Move()
    {
        Vector3 input = master.arrowInput * Time.deltaTime * walkAcceleration;

        Fall();

        if (master.horizontalSpeed.sqrMagnitude < walkSpeed * walkSpeed)
        {

        }
        master.SPEED += input;

        if (master.grounded)
        {
            if (input == Vector3.zero)
            {
                master.SPEED -= master.horizontalSpeed.normalized * Time.deltaTime * walkDeceleration;
            }

            if (Input.GetKey(KeyCode.Space)) { master.SPEED.y = jumpForce; master.grounded = false; }
        }
    }

    void Fall()
    {
        master.SPEED.y -= master.gravity * Time.deltaTime;
    }

    public void Collide(ControllerColliderHit hit)
    {
        Vector3 collisionForce = Vector3.Project(master.SPEED, hit.normal);

        master.SPEED -= collisionForce;
    }

    private void OnDrawGizmos()
    {
        Vector3 startPos, normal, force, result;
        startPos = new Vector3(0, 10, 0);
        force = new Vector3(0, -2, 0);
        normal = new Vector3(1, 1, 0);

        result = normal;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(startPos, force);
        Gizmos.color = Color.white;
        Gizmos.DrawRay(startPos, normal);
        Gizmos.color = Color.black;
        Gizmos.DrawRay(startPos, result);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(startPos, force + result);
    }
}
