using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && player.Skill.Blackhole.BlackHoleUnlocked)
        {
            if (player.Skill.Blackhole.CooldownTimer > 0)
            {
                player.Fx.CreatePopUpText("Cooldown");
                return;
            }

            stateMachine.ChangeState(player.BlackHole);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1)&&HasNoSword()&&player.Skill.Sword.SwordUnlocked)
            stateMachine.ChangeState(player.AimSwordState);

        if (Input.GetKeyDown(KeyCode.Q) && player.Skill.Parry.ParryUnlocked)
            stateMachine.ChangeState(player.CounterAttackState);

        if (Input.GetKey(KeyCode.Mouse0))
            stateMachine.ChangeState(player.PrimaryAttackState);

        if (player.IsGroundDetected() == false)
            stateMachine.ChangeState(player.AirState);

        if (Input.GetKeyDown(KeyCode.Space)&&player.IsGroundDetected())
            stateMachine.ChangeState(player.JumpState);
    }

    private bool HasNoSword()
    {
        if (!player.Sword)
        {
            return true;
        }

        player.Sword.GetComponent<SwordSkillController>().ReturnSword();
        return false; 
    }
}
