using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Stunned info")]
    [SerializeField] protected GameObject counterImage;
    public float StunDuration;
    public Vector2 StunDirection;
    protected bool canBeStunned;

    [Header("Move info")]
    public float MoveSpeed;
    public float IdleStateDuration;
    public float BattleTime;

    [Header("Attack info")]
    public float AttackDistance;
    public float AttackCooldown;

    [HideInInspector] public float LastTimeAttacked;

    public EnemyStateMachine StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();

    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDir, 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + AttackDistance * FacingDir, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    
}
