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
    private float _defaultMoveSpeed;

    [Header("Attack info")]
    public float AttackDistance;
    public float AttackCooldown;

    [HideInInspector] public float LastTimeAttacked;

    public EnemyStateMachine StateMachine { get; private set; }

    public string LastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        _defaultMoveSpeed = MoveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();

    }

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        LastAnimBoolName = _animBoolName;
    }

    public virtual void FreezeTime(bool timeFrozen)
    {
        if (timeFrozen)
        {
            MoveSpeed = 0;
            Anim.speed = 0;
        }
        else
        {
            MoveSpeed = _defaultMoveSpeed;
            Anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(seconds);

        FreezeTime(false);
    }

    #region Counter Attack  Window
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
    #endregion
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
