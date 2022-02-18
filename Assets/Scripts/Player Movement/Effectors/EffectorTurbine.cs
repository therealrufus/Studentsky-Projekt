using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorTurbine : PlayerEffector
{
    public float speedIncrease;

    private void OnTriggerStay(Collider other)
    {
        if (GetPlayer(other.gameObject))
        {
            playerMovement.SPEED += Vector3.up * speedIncrease * Time.deltaTime * 100;
        }
    }
}
