using System;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType SwordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int _bounceAmount;
    [SerializeField] private float _bounceGravity;
    [SerializeField] private float _bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private int _pierceAmount;
    [SerializeField] private float _pierceGravity;

    [Header("Spin info")]
    [SerializeField] private float _hitCooldown = 0.35f;
    [SerializeField] private float _maxTravelDistance = 7;
    [SerializeField] private float _spingDuration=2;
    [SerializeField] private float _spinGravity=1;
    
    [Header("Skill info")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravity;
    [SerializeField] private float _freezeTimeDuration;
    [SerializeField] private float _returnSpeed;

    private Vector2 _finalDirection;

    [Header("Aim dots")]
    [SerializeField] private int _numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private Transform _dotsParent;
    [SerializeField] private GameObject _dotPrefab;

    private GameObject[] _dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();

        SetupGravity();
    }

    private void SetupGravity()
    {
        if (SwordType == SwordType.Bounce)
            _swordGravity = _bounceGravity;
        else if (SwordType == SwordType.Pierce)
            _swordGravity = _pierceGravity;
        else if (SwordType == SwordType.Spin)
            _swordGravity = _spinGravity;
    }

    protected override void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse1))
            _finalDirection = new Vector2(AimDirection().normalized.x * _launchForce.x, AimDirection().normalized.y * _launchForce.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < _dots.Length; i++)
            {
                _dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(_swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (SwordType == SwordType.Bounce)
            newSwordScript.SetupBounce(true, _bounceAmount,_bounceSpeed);
        else if (SwordType == SwordType.Pierce)
            newSwordScript.SetupPierce(_pierceAmount);
        else if (SwordType == SwordType.Spin)
            newSwordScript.SetupSpin(true,_maxTravelDistance,_spingDuration,_hitCooldown);

        newSwordScript.SetupSword(_finalDirection, _swordGravity, player,_freezeTimeDuration,_returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    #region Aim 

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    private void GenerateDots()
    {
        _dots = new GameObject[_numberOfDots];

        for (int i = 0; i < _numberOfDots; i++)
        {
            _dots[i] = Instantiate(_dotPrefab, player.transform.position, Quaternion.identity, _dotsParent);
            _dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < _dots.Length; i++)
        {
            _dots[i].SetActive(_isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(AimDirection().normalized.x * _launchForce.x,
            AimDirection().normalized.y * _launchForce.y) * t + 0.5f * (Physics2D.gravity * _swordGravity) * (t * t);

        return position;
    }

    #endregion
}
