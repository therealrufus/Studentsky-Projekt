using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveScript : MonoBehaviour
{
    public Rigidbody rb;
    public Transform start;
    public Transform end;
    public float speed;

    private void Update()
    {
        float t = Mathf.Sin(speed * Time.time);
        t = (t + 1) / 2;
        //transform.position = Vector3.Lerp(start.position, end.position, t);
        rb.MovePosition(Vector3.Lerp(start.position, end.position, t));
    }
}
