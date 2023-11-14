using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy baseEnemy, EnemyStateMachine stateMachine, string animName, EnemySkeleton enemy) : base(baseEnemy, stateMachine, animName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer < 0f)
        {
            stateMachine.ChangeState(enemy.moveState);
        }       
        
    }


}
