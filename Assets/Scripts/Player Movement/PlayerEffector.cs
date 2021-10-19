using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffector : MonoBehaviour
{
    protected PlayerMovement playerMovement;

    protected bool GetPlayer(GameObject player)
    {
        if (playerMovement != null) return true;
        playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement != null) return true;
        else return false;
    }

    protected virtual void Subscribe()
    {
        if (playerMovement != null) playerMovement.activeEffectors.Add(this);
    }

    public virtual Vector3 Move()
    {
        return Vector3.zero;
    }

    public virtual Vector3 ConstantMove()
    {
        return Vector3.zero;
    }

    protected virtual void UnSubscribe()
    {
        if(playerMovement != null) playerMovement.activeEffectors.Remove(this);
    }
}
