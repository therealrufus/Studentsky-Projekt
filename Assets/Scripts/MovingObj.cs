using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObj : PlayerEffector
{
    Rigidbody rb;
    public Transform start;
    public Transform end;
    public float moveSpeed;

    //must be multiplied by Time.deltatime!!
    Vector3 SPEED;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("hello");
        if (GetPlayer(collision.gameObject))
        {
            Subscribe();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        UnSubscribe();
        Debug.LogError("AAAAAA");
    }

    private void Update()
    {
        GetSpeed();
        //rb.MovePosition(transform.position + SPEED);
        transform.position += SPEED;
    }

    public override Vector3 ConstantMove()
    {
        return SPEED;
    }

    public virtual void GetSpeed()
    {
        float t = Mathf.Sin(moveSpeed * Time.time);
        t = (t + 1) / 2;
        Vector3 speed = Vector3.Lerp(start.position, end.position, t) - transform.position;
        SPEED = speed;
    }
}
