using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if(Input.GetKeyDown(KeyCode.X) && HasSword())
        {
            stateMachine.ChangeState(player.aimSwordState);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.primaryAttack);
        }

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.counterAttackState);
        }
    }
    
    public bool HasSword()
    {
        if(player.sword == null)
        {
            return true;
        }

        var swordController = player.sword.GetComponent<SwordSkillController>();
        swordController.ReturnSword();

        return false;
    }
}
