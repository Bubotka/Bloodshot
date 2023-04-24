using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    public int ComboCounter { get; private set; }

    private float _lastTimeAttacked;
    private float _comboWindow=2;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
        : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        xInput = 0; //need to fix bug on attack direction

        if (ComboCounter > 2||Time.time >=_lastTimeAttacked+_comboWindow)
            ComboCounter = 0;

        player.Anim.SetInteger("ComboCounter", ComboCounter);

        float attackDir = player.FacingDir;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.AttackMovement[ComboCounter].x*attackDir, player.AttackMovement[ComboCounter].y);

        stateTimer=0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);

        ComboCounter++;

        _lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();

        if (triggerCalled == true)
            stateMachine.ChangeState(player.IdleState);
    }
}
