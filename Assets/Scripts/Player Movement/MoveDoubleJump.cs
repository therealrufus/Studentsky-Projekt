using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDoubleJump : PlayerMoveOption
{
    [Space(20)]
    [SerializeField] float JumpForce;

    [Space]
    [SerializeField] MoveBasic moveBasic;
    [SerializeField] MoveWallride moveWallride;
    bool canJump;

    private void Update()
    {
        if (master.grounded) canJump = true;
    }

    public override void Inicialize()
    {
        moveWallride.OnEnd.AddListener(ResetJump);
    }

    public override bool ShouldStart()
    {
        if (!master.grounded && master.groundedForFrames > 5)
        {
            if (canJump && Input.GetKeyDown(KeyCode.Space))
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
