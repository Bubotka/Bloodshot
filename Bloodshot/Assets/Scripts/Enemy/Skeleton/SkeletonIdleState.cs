using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemy) 
        : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();

        stateTimer = enemy.IdleStateDuration;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.PlaySFX(37,enemy.transform);
    } 
}
