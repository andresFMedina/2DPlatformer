using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && player.facingDirection != xInput)
        {
            stateMachine.ChangeState(player.idleState);
        }

        rb.velocity = (yInput >= 0) ?
            new Vector2(0, rb.velocity.y * 0.7f) :
            new Vector2(0, rb.velocity.y);        

        if(player.IsGroundDetected()) stateMachine.ChangeState(player.idleState);
    }
}
