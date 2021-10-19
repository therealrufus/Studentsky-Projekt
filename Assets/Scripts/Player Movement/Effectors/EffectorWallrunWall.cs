using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorWallrunWall : PlayerEffector
{
    public float speed;

    public override Vector3 ConstantMove()
    {
        Vector3 force = Vector3.zero;
        MoveWallride wallRide = playerMovement.currentMoveOption as MoveWallride;
        if (wallRide != null)
        {
            Vector3 normal  = Quaternion.AngleAxis(-90, Vector3.up) * wallRide.normal;

            Vector3 speedOnWall = Vector3.Project(playerMovement.SPEED.normalized, normal);
            force =  speedOnWall.normalized * (speed);

            playerMovement.SPEED.y = 0;
        }

        return force;
    }

    private void OnTriggerEnter(Collider other)
    {
        GetPlayer(other.gameObject);
        Subscribe();
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerMovement != null)
        {
            Vector3 force = Vector3.zero;
            MoveWallride wallRide = playerMovement.currentMoveOption as MoveWallride;
            if (wallRide != null)
            {
                Vector3 normal = Quaternion.AngleAxis(-90, Vector3.up) * wallRide.normal;

                Vector3 speedOnWall = Vector3.Project(playerMovement.SPEED.normalized, normal);
                force = speedOnWall.normalized * (speed);
            }
            playerMovement.SPEED += force;
        }
        UnSubscribe();
    }
}
