using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float _flyTime=0.4f;
    private bool _skillUsed;
    private float _defaultGravity;

    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _defaultGravity = player.Rb.gravityScale;

        _skillUsed = false;
        stateTimer = _flyTime;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.Rb.gravityScale = _defaultGravity;
        player.Fx.MakeTransparent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 8);

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -0.1f);

            if (!_skillUsed)
            {
               if( player.Skill.Blackhole.CanUseSkill())
                _skillUsed = true;
            }
        }

        if (player.Skill.Blackhole.SkillCompleted())
            stateMachine.ChangeState(player.AirState);
    }
}
