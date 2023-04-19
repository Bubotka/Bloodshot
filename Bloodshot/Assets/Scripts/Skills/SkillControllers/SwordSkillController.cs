using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private float _returnSpeed = 12;
    private Animator _anim;
    private Rigidbody2D _rb;
    private CircleCollider2D _cd;
    private Player _player;

    private bool _canRotate = true;
    private bool _isReturning;
    private float _freezeTimeDuration;

    [Header("Pierce info")]
    private float _pierceAmount;

    [Header("Bounce info")]
    private float _bounceSpeed = 14;
    private List<Transform> _enemyTarget;
    private bool _isBouncing;
    private int _BounceAmount;
    private int _targetIndex;

    [Header("Spin info")]
    private float _maxTravelDistance;
    private float _spinDuration;
    private float _spinTimer;
    private bool _wasStopped;
    private bool _isSpinning;
    private float _hitTimer;
    private float _hitCooldown;

    private float _spinDirection;
    private float _swordSpinSpeed=1f;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player player, float freezeTimeDuration, float returnSpeed)
    {
        _player = player;
        _rb.velocity = dir;
        _rb.gravityScale = gravityScale;
        _freezeTimeDuration = freezeTimeDuration;
        _returnSpeed = returnSpeed;

        _anim.SetBool("Rotation", true);

        _spinDirection = Mathf.Clamp(_rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 7);
    }

    public void SetupBounce(bool isBouncing, int amountOfBounces, float bounceSpeed)
    {
        _enemyTarget = new List<Transform>();

        _isBouncing = isBouncing;
        _BounceAmount = amountOfBounces;
        _bounceSpeed = bounceSpeed;
    }
    public void SetupPierce(int pierceAmount)
    {
        _pierceAmount = pierceAmount;
    }

    public void SetupSpin(bool isSpinning, float maxTravelDistance, float spinDuration, float hitCooldown)
    {
        _isSpinning = isSpinning;
        _maxTravelDistance = maxTravelDistance;
        _spinDuration = spinDuration;
        _hitCooldown = hitCooldown;
    }

    public void ReturnSword()
    {
        _anim.SetBool("Rotation", true);
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        _isReturning = true;
        _cd.enabled = true;
    }

    private void Update()
    {
        if (_canRotate)
            transform.right = _rb.velocity;

        if (_isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _player.transform.position) < 1)
                _player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();
    }

    private void SpinLogic()
    {
        if (_isSpinning)
        {
            if (Vector2.Distance(_player.transform.position, transform.position) > _maxTravelDistance && !_wasStopped)
            {
                StopWhenSpinning();
            }

            if (_wasStopped)
            {
                _spinTimer -= Time.deltaTime;

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + _spinDirection, transform.position.y), _swordSpinSpeed * Time.deltaTime);

                if (_spinTimer < 0)
                {
                    _isReturning = true;
                    _isSpinning = false;
                }

                _hitTimer -= Time.deltaTime;

                if (_hitTimer < 0)
                {
                    _hitTimer = _hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        _wasStopped = true;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        _spinTimer = _spinDuration;
    }

    private void BounceLogic()
    {
        if (_isBouncing && _enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, _enemyTarget[_targetIndex].position, _bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _enemyTarget[_targetIndex].position) < 0.1f)
            {
                SwordSkillDamage(_enemyTarget[_targetIndex].GetComponent<Enemy>());

                _targetIndex++;
                _BounceAmount--;

                if (_BounceAmount <= 0)
                {
                    _isBouncing = false;
                    _isReturning = true;
                }

                if (_targetIndex >= _enemyTarget.Count)
                    _targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            SwordSkillDamage(enemy);
        }

        SetUpTargetsForBounce(collision);

        if (!_isReturning)
            StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.DamageEffect();
        enemy.StartCoroutine("FreezeTimeFor", _freezeTimeDuration);
    }

    private void SetUpTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (_isBouncing && _enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        _enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (_pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            _pierceAmount--;
            return;
        }

        if (_isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        _canRotate = false;
        _cd.enabled = false;

        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (_isBouncing && _enemyTarget.Count > 0)
            return;

        _anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
