using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.Skill.Clone.CreateCloneOnDashStart();

        stateTimer = player.DashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.Skill.Clone.CreateCloneOnDashOver();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.WallSlideState);

        player.SetVelocity(player.DashSpeed * player.DashDiraction, 0);

        if (stateTimer < 0)
            stateMachine.ChangeState(player.IdleState);
    }
}
