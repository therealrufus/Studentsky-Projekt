using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPizza : MonoBehaviour
{
    public PlayerMovement movement;
    public Transform camera;
    public GameObject prefab;
    public float force;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject pizza = Instantiate(prefab, transform.position, transform.rotation);
        pizza.GetComponent<Rigidbody>().AddForce(movement.SPEED + camera.forward * force);
    }
}
