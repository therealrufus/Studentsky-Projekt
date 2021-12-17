using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoubleJump : PlayerMoveOption
{
    [Space(20)]
    [SerializeField, Tooltip("the force of the jump")] float JumpForce;

    [Space]
    [SerializeField, Tooltip("when to recharge the double jump")] MoveWallride moveWallride;
    [SerializeField, Tooltip("when to recharge the double jump")] MoveGrapple moveGrapple;
    [SerializeField, Tooltip("when to recharge the double jump")] MoveKick moveKick;
    bool canJump;

    private void Update()
    {
        if (master.grounded) canJump = true;
    }

    public override void Inicialize()
    {
        moveWallride?.OnEnd.AddListener(ResetJump);
        moveGrapple?.OnEnd.AddListener(ResetJump);
        moveKick?.OnJump.AddListener(ResetJump);
    }



    public override bool ShouldStart()
    {
        if (!master.grounded && master.groundedForFrames > 5)
        {
            if (canJump && Input.GetKeyDown(InputManager.Jump))
            {
                return true;
            }
        }

        if (master.grounded) canJump = true;

        return false;
    }

    public override void Move()
    {
        base.Move();

        if (master.SPEED.y < 0) master.SPEED.y = 0;
        master.SPEED += Vector3.up * JumpForce;
        canJump = false;

        Fall();

        OnJump.Invoke();
    }

    void ResetJump()
    {
        Invoke("HardResetJump", 0.1f);
    }

    void HardResetJump()
    {
        canJump = true;
    }
}
