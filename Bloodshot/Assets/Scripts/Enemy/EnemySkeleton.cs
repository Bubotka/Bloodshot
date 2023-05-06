using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Enemy
{
    #region States
    public  SkeletonIdleState IdleState { get; private set; }

    public SkeletonMoveState MoveState { get; private set; }

    public SkeletonBattleState BattleState { get; private set; }

    public SkeletonAttackState AttackState { get; private set; }

    public SkeletonStunnedState StunnedState { get; private set; }

    public SkeletonDeadState DeadState { get; private set; }
    #endregion 

    private float _sfxTimer;
    private float _sfxCooldown=15;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new SkeletonIdleState(this, StateMachine, "Idle", this);
        MoveState = new SkeletonMoveState(this, StateMachine, "Move", this);
        BattleState = new SkeletonBattleState(this, StateMachine, "Move", this);
        AttackState = new SkeletonAttackState(this, StateMachine, "Attack", this);
        StunnedState = new SkeletonStunnedState(this, StateMachine, "Stunned", this);
        DeadState = new SkeletonDeadState(this, StateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();

        _sfxTimer -= Time.deltaTime;

        if (_sfxTimer <= 0)
        {
            AudioManager.Instance.PlaySFX(20,transform);

            _sfxTimer = _sfxCooldown;
        }
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            StateMachine.ChangeState(StunnedState);
            return true;
        }

        return false;
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }
}
