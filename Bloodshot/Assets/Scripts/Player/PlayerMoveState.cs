using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.Instance.PlaySFX(15,null);
    }

    public override void Exit()
    {
        base.Exit();

        AudioManager.Instance.StopSFX(15);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput*player.MoveSpeed, rb.velocity.y);

        if (xInput == 0||player.IsWallDetected())
            stateMachine.ChangeState(player.IdleState);
    }
}
