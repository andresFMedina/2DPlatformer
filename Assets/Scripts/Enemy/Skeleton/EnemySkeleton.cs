
public class EnemySkeleton : Enemy
{
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }

    public SkeletonStunnedState stunnedState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        idleState = new(this, stateMachine, "Idle", this);
        moveState = new(this, stateMachine, "Move", this);
        battleState = new(this, stateMachine, "Move", this);
        attackState = new(this, stateMachine, "Attack", this);
        stunnedState = new(this, stateMachine, "Stunned", this);
    }


    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if(base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }
}
