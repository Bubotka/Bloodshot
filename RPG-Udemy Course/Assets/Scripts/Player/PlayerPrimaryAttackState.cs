using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int _comboCounter;
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

        if (_comboCounter > 2||Time.time >=_lastTimeAttacked+_comboWindow)
            _comboCounter = 0;

        player.Anim.SetInteger("ComboCounter", _comboCounter);

        float attackDir = player.FacingDir;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.AttackMovement[_comboCounter].x*attackDir, player.AttackMovement[_comboCounter].y);

        stateTimer=0.1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.15f);

        _comboCounter++;

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
