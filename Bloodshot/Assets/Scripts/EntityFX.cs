using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer _sr;

    [Header("Flash FX")]
    [SerializeField] private float _flashDuration;
    [SerializeField] private Material _hitMat;
    private Material _originalMat;

    [Header("Aliment colorts")]
    [SerializeField] private Color[] _chillColor;
    [SerializeField] private Color[] _igniteColor;
    [SerializeField] private Color[] _shockColor;

    private void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _originalMat = _sr.material;
    }

    private IEnumerator FlashFX()
    {
        _sr.material = _hitMat;
        Color currentColor = _sr.color;

        _sr.color = Color.white;
        yield return new WaitForSeconds(_flashDuration);

        _sr.color = currentColor;
        _sr.material = _originalMat;


    }

    private void RedColorBlink()
    {
        if (_sr.color != Color.white)
            _sr.color = Color.white;
        else
            _sr.color = Color.red;
    }

    private void Cancel�olorChange()
    {
        CancelInvoke();
        _sr.color = Color.white;
    }

    public void ChillFxFor(float seconds)
    {
        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("Cancel�olorChange", seconds);
    }

    public void IgniteFxFor(float seconds)
    {
        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("Cancel�olorChange", seconds);
    }

    public void ShockFxFor(float seconds)
    {
        InvokeRepeating("ShockColorFX", 0, 0.15f);
        Invoke("Cancel�olorChange", seconds);
    }

    private void IgniteColorFX()
    {
        if (_sr.color != _igniteColor[0])
            _sr.color = _igniteColor[0];
        else
            _sr.color = _igniteColor[1];
    }

    private void ShockColorFX()
    {
        if (_sr.color != _shockColor[0])
            _sr.color = _shockColor[0];
        else
            _sr.color = _shockColor[1];
    }

    private void ChillColorFX()
    {
        if (_sr.color != _chillColor[0])
            _sr.color = _chillColor[0];
        else
            _sr.color = _chillColor[1];
    }
}
