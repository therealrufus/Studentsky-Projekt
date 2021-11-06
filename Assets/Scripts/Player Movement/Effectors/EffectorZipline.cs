using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorZipline : PlayerEffector
{
    public Transform pointA;
    public Transform pointB;
    public LineRenderer line;
    BoxCollider col;
    [SerializeField]
    float colliderSize = 0.1f;
    [SerializeField]
    Vector2 offset;
    [SerializeField]
    float clampForce = 10f;
    [SerializeField]
    float speedBonus = 5f;
    [SerializeField]
    float jumpForce = 20f;

    Vector3 normal;
    bool hooked;

    private void Awake()
    {
        Inicialize();
    }

    void Inicialize()
    {
        col = GetComponent<BoxCollider>();
        line.positionCount = 2;
        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);

        //http://www.theappguruz.com/blog/add-collider-to-line-renderer-unity thx Swati Panel
        col.size = new Vector3(colliderSize, colliderSize, Vector3.Distance(pointA.position, pointB.position));
        Vector3 midPoint = (pointA.position + pointB.position) / 2;
        col.transform.position = midPoint;

        col.transform.LookAt(pointA, transform.up);
        normal = pointB.position - pointA.position;
        normal = normal.normalized;

        hooked = false;
    }

    public override Vector3 ConstantMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Invoke("UnSubscribe", 0.001f);
            playerMovement.SPEED += Vector3.up * jumpForce;
            hooked = false;
            return Vector3.zero;
        }

        Vector3 positionOnLine = GetClosestPointLine(playerMovement.transform.position, pointA.position, pointB.position);
        positionOnLine += Vector3.Cross(normal, Vector3.up) * offset.x;
        positionOnLine += Vector3.Cross(normal, Vector3.Cross(normal, Vector3.up)) * -1 * offset.y;
        Vector3 clamp = (positionOnLine - playerMovement.transform.position) * clampForce;
        return clamp;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.E) && !hooked)
        {
            GetPlayer(other.gameObject);
            Subscribe();
            if (playerMovement != null)
            {
                Vector3 speed = Vector3.Project(playerMovement.SPEED, normal);
                if (speed.sqrMagnitude < speedBonus * speedBonus)
                    speed += speed.normalized * speedBonus;
                playerMovement.SPEED = speed;
                hooked = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        UnSubscribe();
        hooked = false;
    }

    Vector3 GetClosestPointLine(Vector3 point, Vector3 line_start, Vector3 line_end)
    {
        return line_start + Vector3.Project(point - line_start, line_end - line_start);
    }


    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.2f);
            Gizmos.DrawSphere(pointB.position, 0.2f);
        }
    }
}
