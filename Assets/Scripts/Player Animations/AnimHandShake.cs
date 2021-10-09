using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandShake : PlayerEffect
{
    [SerializeField] MoveBasic moveBasic;
    [SerializeField] float shakeSpeed;
    [SerializeField] float shakeMagnitude;
    [SerializeField] float minimumSpeed;

    Transform handLeft;
    Transform handRight;

    private void Start()
    {
        moveBasic.OnMove.AddListener(Move);
        handLeft = SpawnHolder(master.handLeft);
        handRight = SpawnHolder(master.handRight);
    }

    void Move()
    {
        master.RotateWithEase(master.handLeft, Quaternion.identity, true);

        if (master.playerMovement.SPEED.sqrMagnitude > minimumSpeed * minimumSpeed && master.playerMovement.grounded && master.playerMovement.arrowInput != Vector3.zero)
        {
            float shakeAmmount = Mathf.Sin(shakeSpeed * Time.time) * shakeMagnitude;
            master.MoveWithEase(master.handLeft, new Vector3(0, shakeAmmount, 0), true);
            master.MoveWithEase(master.handRight, new Vector3(0, -shakeAmmount, 0), true);
        }
        else
        {
            master.MoveWithEase(master.handLeft, Vector3.zero, true);
            master.MoveWithEase(master.handRight, Vector3.zero, true);
        }
    }
}
