using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorJumpPad : PlayerEffector
{
    public float JumpForce;

    private void OnTriggerEnter(Collider other)
    {
        if (GetPlayer(other.gameObject))
        {
            playerMovement.SPEED = new Vector3(playerMovement.SPEED.x, Mathf.Abs(playerMovement.lastFrameSpeed.y), playerMovement.SPEED.z);
            playerMovement.SPEED += Vector3.up * JumpForce;
        }
    }
}
