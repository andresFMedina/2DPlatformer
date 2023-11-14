using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private EnemySkeleton enemy;
    private Transform player;
    private int moveDir;

    public SkeletonBattleState(Enemy baseEnemy, EnemyStateMachine stateMachine, string animName, EnemySkeleton enemy) : base(baseEnemy, stateMachine, animName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(CanAttack()) stateMachine.ChangeState(enemy.attackState);
                return;
            }
        }
        else
        {
            if(stateTimer < 0  || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        moveDir = (player.position.x > enemy.transform.position.x) ? 1 : -1;

        enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);

    }

    private bool CanAttack()
    {
        if(Time.time >= enemy.lastTimeAttack + enemy.attackCooldown)
        {
            enemy.lastTimeAttack = Time.time;
            return true;
        }

        return false;
    }
}
