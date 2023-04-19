using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform _sword; 

    public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _sword = player.Sword.transform;

        if (player.transform.position.x > _sword.position.x && player.FacingDir == 1)
            player.Flip();
        else if (player.transform.position.x < _sword.position.x && player.FacingDir == -1)
            player.Flip();

        rb.velocity = new Vector2(player.SwordReturnImpact * -player.FacingDir, rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
