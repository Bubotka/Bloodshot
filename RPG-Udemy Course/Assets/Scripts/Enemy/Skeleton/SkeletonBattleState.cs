using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform _player;
    private EnemySkeleton _enemy;
    private int _moveDir;

    public SkeletonBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, EnemySkeleton enemy)
        : base(enemyBase, stateMachine, animBoolName)
    {
        _enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        _player = GameObject.Find("Player").transform;
    }


    public override void Update()
    {
        base.Update();

        if (_enemy.IsPlayerDetected())
        {
            stateTimer = _enemy.BattleTime;

            if (_enemy.IsPlayerDetected().distance < _enemy.AttackDistance)
            {
                if(CanAttack())
                    stateMachine.ChangeState(_enemy.AttackState);
            }
        }
        else
        {
            if (stateTimer < 0||Vector2.Distance(_player.transform.position, _enemy.transform.position)>10)
                stateMachine.ChangeState(_enemy.IdleState);
        }

        if (_player.position.x > _enemy.transform.position.x)
            _moveDir = 1;
        else if (_player.position.x < _enemy.transform.position.x)
            _moveDir = -1;

        if (_enemy.IsPlayerDetected()&&Vector2.Distance(_player.transform.position, _enemy.transform.position) <= _enemy.AttackDistance)
            _enemy.SetZeroVelocity();
        else
            _enemy.SetVelocity(_enemy.MoveSpeed * _moveDir, rb.velocity.y);
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= _enemy.LastTimeAttacked + _enemy.AttackCooldown)
        {
            _enemy.LastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
