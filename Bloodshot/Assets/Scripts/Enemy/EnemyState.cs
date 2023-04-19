using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected float stateTimer; 
    protected bool triggerCalled;
    protected Rigidbody2D rb;

    private string _animBoolName;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        _animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemyBase.Anim.SetBool(_animBoolName, true);
        rb = enemyBase.Rb;
    }

    public virtual void Exit()
    {
        enemyBase.Anim.SetBool(_animBoolName, false);
        enemyBase.AssignLastAnimName(_animBoolName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
