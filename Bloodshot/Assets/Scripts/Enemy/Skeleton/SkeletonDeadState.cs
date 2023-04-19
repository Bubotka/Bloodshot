using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private EnemySkeleton _enemy;

    public SkeletonDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName,EnemySkeleton enemy) 
        : base(enemyBase, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        _enemy.Anim.SetBool(_enemy.LastAnimBoolName, true);
        _enemy.Anim.speed = 0;
        _enemy.Cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 5);
    }
}
