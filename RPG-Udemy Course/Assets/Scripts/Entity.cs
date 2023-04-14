using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Entity : MonoBehaviour
{
    #region Components
    public Animator Anim { get; private set; }

    public Rigidbody2D Rb { get; private set; }

    public EntityFX FX { get; private set; }
    #endregion

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;


    [Header("Collision info")]
    public Transform AttackCheck;
    public float AttackCheckRaduis;
    [SerializeField] protected Transform _groundCheck;
    [SerializeField] protected float _groundCheckDistance;
    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected float _wallCheckDistance;
    [SerializeField] protected LayerMask _whatIsGround;

    public int FacingDir { get; private set; } = 1;
    protected bool _facingRight = true;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        FX = GetComponent<EntityFX>();
        Anim = GetComponentInChildren<Animator>();
        Rb = GetComponent<Rigidbody2D>();
    }
    
    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        FX.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");

        Debug.Log(gameObject.name + "was damaged");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        Rb.velocity = new Vector2(knockbackDirection.x * -FacingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.Raycast(_wallCheck.position, Vector2.right * FacingDir, _wallCheckDistance, _whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(_groundCheck.position, new Vector3(_groundCheck.position.x, _groundCheck.position.y - _groundCheckDistance));
        Gizmos.DrawLine(_wallCheck.position, new Vector3(_wallCheck.position.x + _wallCheckDistance * FacingDir, _wallCheck.position.y));
        Gizmos.DrawWireSphere(AttackCheck.position,AttackCheckRaduis);
    }
    #endregion

    #region Flip
    public virtual void Flip()
    {
        FacingDir *= -1;
        _facingRight = !_facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float x)
    {
        if (x > 0 && !_facingRight)
            Flip();
        else if (x < 0 && _facingRight)
            Flip();
    }
    #endregion

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        Rb.velocity = Vector2.zero;
    } 

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked)
            return;

        Rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
    #endregion

}
