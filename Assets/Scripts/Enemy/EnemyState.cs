using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected Enemy baseEnemy;
    protected EnemyStateMachine stateMachine;
    protected string animName;

    protected bool triggerCalled;
    protected float stateTimer;

    public EnemyState(Enemy baseEnemy, EnemyStateMachine stateMachine, string animName)
    {
        this.baseEnemy = baseEnemy;
        this.stateMachine = stateMachine;
        this.animName = animName;
        
    }

    public virtual void Enter()
    {
        triggerCalled = false;        
        baseEnemy.animator.SetBool(animName, true);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        baseEnemy.animator.SetBool(animName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
