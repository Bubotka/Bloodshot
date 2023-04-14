using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton _enemy;

    public SkeletonAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemy)
        : base(enemyBase, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        _enemy.LastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        _enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(_enemy.BattleState);
    }
}
