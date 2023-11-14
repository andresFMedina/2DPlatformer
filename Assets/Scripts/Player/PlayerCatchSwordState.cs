using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{

    private Transform sword;

    public PlayerCatchSwordState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if (player.transform.position.x > sword.position.x && player.facingDirection == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < sword.position.x && player.facingDirection == -1)
        {
            player.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 1f);
    }

    public override void Update()
    {
        base.Update();
        if(triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }    
}
