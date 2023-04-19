using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool _canCreateClone;

    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _canCreateClone = true;
        stateTimer = player.counterAttackDuration;
        player.Anim.SetBool("SuccesfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackCheckRaduis);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.Anim.SetBool("SuccesfulCounterAttack", true);

                    if (_canCreateClone)
                    {
                        _canCreateClone = false;
                        player.Skill.Clone.CreateCloneOnCounterAttack(hit.transform);
                    }

                }
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.IdleState);
    }
}
