using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerFX : EntityFX
{
    [Header("After image fx")]
    [SerializeField] private float _afterImageCooldown;
    [SerializeField] private GameObject _afterImagePrefab;
    [SerializeField] private float _colorLooseRate;
    private float _afterImageCooldownTimer;

    [Header("Screen shake fx")]
    private CinemachineImpulseSource _screenShake;
    [SerializeField] private float _shakeMultiplier;
    public Vector3 _shakeSwordImpact;
    public Vector3 _shakeHighDamage;

    protected override void Start()
    {
        base.Start();
        _screenShake = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        _afterImageCooldownTimer -= Time.deltaTime;
    }

    public void ScreenShake(Vector3 shakePower)
    {
        _screenShake.m_DefaultVelocity = new Vector2(shakePower.x * _player.FacingDir, shakePower.y) * _shakeMultiplier;
        _screenShake.GenerateImpulse();
    }

    public void CreateAfterImage()
    {
        if (_afterImageCooldownTimer < 0)
        {
            _afterImageCooldownTimer = _afterImageCooldown;
            GameObject newAfterImage = Instantiate(_afterImagePrefab, transform.position, transform.rotation);
            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(_colorLooseRate, _sr.sprite);
        }
    }
}
