using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class NetworkPlayerMovement : PlayerMovement
{
    public static GameObject LocalPlayerInstance;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = gameObject;
        }
    }

    protected override void StartMovement()
    {
        if (photonView.IsMine)
        {
            /*cam = Camera.main.transform;
            cam.parent = camOffset;
            cam.localPosition = Vector3.zero;*/

            base.StartMovement();
        }
    }

    protected override void UpdateMovement()
    {
        if (photonView.IsMine)
        {
            base.UpdateMovement();
        }
    }
}
