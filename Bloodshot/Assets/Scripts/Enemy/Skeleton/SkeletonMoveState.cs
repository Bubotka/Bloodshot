using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{

    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(41, enemy.transform);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.StopSFX(41);
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.MoveSpeed * enemy.FacingDir, rb.velocity.y);

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.IdleState);
        }

    }
}
