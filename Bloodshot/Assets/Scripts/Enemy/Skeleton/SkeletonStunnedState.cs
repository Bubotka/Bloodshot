using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton _enemy;

    public SkeletonStunnedState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemy) 
        : base(enemyBase, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.Fx.InvokeRepeating("RedColorBlink", 0, 0.1f);

        stateTimer = _enemy.StunDuration ;

        rb.velocity= new Vector2(-_enemy.FacingDir * _enemy.StunDirection.x, _enemy.StunDirection.y);

        _enemy.Invoke("SetZeroVelocity", 0.2f);

        AudioManager.Instance.PlaySFX(44, _enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();

        _enemy.Fx.Invoke("Cancel�olorChange", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(_enemy.IdleState);
    }
}
