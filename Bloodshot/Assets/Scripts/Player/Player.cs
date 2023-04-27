using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public bool IsBusy { get; private set; }

    [Header("Attack info")]
    public Vector2[] AttackMovement;
    public float counterAttackDuration = 0.2f;


    [Header("Move info")]
    public float MoveSpeed = 12f;
    public float JumpForce = 13f;
    public float SwordReturnImpact = 7;
    private float _defaultMoveSpeed;
    private float _defaultJumpForce;
    private float _defaultDashSpeed;


    [Header("Dash info")]
    [SerializeField] private float _dashCooldown;
    private float _dashUsageTimer;
    public float DashSpeed = 20f;
    public float DashDuration = 0.3f;

    public float DashDiraction { get; private set; }

    public SkillManager Skill { get; private set; }

    public GameObject Sword { get; private set; }

    #region States
    public PlayerStateMachine StateMachine { get; private set; }

    public PlayerIdleState IdleState { get; private set; }

    public PlayerMoveState MoveState { get; private set; }

    public PlayerJumpState JumpState { get; private set; }

    public PlayerAirState AirState { get; private set; }

    public PlayerDashState DashState { get; private set; }

    public PlayerWallSlideState WallSlideState { get; private set; }

    public PlayerWallJumpState WallJump { get; private set; }

    public PlayerPrimaryAttackState PrimaryAttackState { get; private set; }

    public PlayerCounterAttackState CounterAttackState { get; private set; }

    public PlayerAimSwordState AimSwordState { get; private set; }

    public PlayerCatchSwordState CatchSwordState { get; private set; }

    public PlayerBlackHoleState BlackHole { get; private set; }

    public PlayerDeadState DeadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        StateMachine = new PlayerStateMachine();

        IdleState = new PlayerIdleState(this, StateMachine, "Idle");
        MoveState = new PlayerMoveState(this, StateMachine, "Move");
        JumpState = new PlayerJumpState(this, StateMachine, "Jump");
        AirState = new PlayerAirState(this, StateMachine, "Jump");
        DashState = new PlayerDashState(this, StateMachine, "Dash");
        WallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        WallJump = new PlayerWallJumpState(this, StateMachine, "Jump");

        PrimaryAttackState = new PlayerPrimaryAttackState(this, StateMachine, "Attack");
        CounterAttackState = new PlayerCounterAttackState(this, StateMachine, "CounterAttack");
        AimSwordState = new PlayerAimSwordState(this, StateMachine, "AimSword");
        CatchSwordState = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        BlackHole = new PlayerBlackHoleState(this, StateMachine, "Jump");
        DeadState = new PlayerDeadState(this, StateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();

        Skill = SkillManager.Instance;

        StateMachine.Initialize(IdleState);

        _defaultJumpForce = JumpForce;
        _defaultMoveSpeed = MoveSpeed;
        _defaultDashSpeed = DashSpeed;
    }

    protected override void Update()
    {
        base.Update();

        StateMachine.CurrentState.Update();

        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.F)&&Skill.Crystal.CrystalUnlocked)
            Skill.Crystal.CanUseSkill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            PlayerInventory.Instance.UseFlask();
            
    }

    public override void SlowEntityBy(float slowPercentage, float slowDuration)
    {
        base.SlowEntityBy(slowPercentage, slowDuration);

        MoveSpeed *= 1 - slowPercentage;
        JumpForce *= 1 - slowPercentage;
        DashSpeed *= 1 - slowPercentage;
        Anim.speed *= 1 - slowPercentage;

        Invoke("ReturnDefaultSpeed", slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        MoveSpeed = _defaultMoveSpeed;
        JumpForce = _defaultJumpForce;
        DashSpeed = _defaultDashSpeed;
    }

    public void AssignNewSword(GameObject newSword)
    {
        Sword = newSword; 
    }

    public void CatchTheSword()
    {
        StateMachine.ChangeState(CatchSwordState);
        Destroy(Sword);
    }

    public void AnimationTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

    public IEnumerator BusyFor(float seconds)
    {
        IsBusy = true;

        yield return new WaitForSeconds(seconds);

        IsBusy = false;
    }
    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;

        if (Skill.Dash.DashUnlocked == false)
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.Dash.CanUseSkill())
        {
            DashDiraction = Input.GetAxisRaw("Horizontal");

            if (DashDiraction == 0)
                DashDiraction = FacingDir;

            StateMachine.ChangeState(DashState);
        }
    }

    public override void Die()
    {
        base.Die();

        StateMachine.ChangeState(DeadState);
    }
}
