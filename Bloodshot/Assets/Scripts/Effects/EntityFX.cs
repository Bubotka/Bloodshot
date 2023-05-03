using Cinemachine;
using JetBrains.Annotations;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class EntityFX : MonoBehaviour
{
    protected SpriteRenderer _sr;
    protected Player _player;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject _popUpTextPrefab;

    [Header("Flash FX")]
    [SerializeField] private float _flashDuration;
    [SerializeField] private Material _hitMat;
    private Material _originalMat;

    [Header("Aliment colorts")]
    [SerializeField] private Color[] _chillColor;
    [SerializeField] private Color[] _igniteColor;
    [SerializeField] private Color[] _shockColor;

    [Header("Element particles")]
    [SerializeField] private ParticleSystem _igniteFx;
    [SerializeField] private ParticleSystem _chillFx;
    [SerializeField] private ParticleSystem _shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject _hitFx;
    [SerializeField] private GameObject _critHitFx;

    protected virtual void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
        _player = PlayerManager.Instance.Player;
        _originalMat = _sr.material;
    }

    private void Update()
    {
        
    }

    public void CreatePopUpText(string text)
    {
        float randomX = Random.Range(-0.75f, 0.75f);
        float randomY = Random.Range(0.25f, 0.75f);

        Vector3 positionOffset = new Vector3(randomX, randomY, 0);

        GameObject newText = Instantiate(_popUpTextPrefab, transform.position + positionOffset, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text = text;
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

    private void Cancel—olorChange()
    {
        CancelInvoke();
        _sr.color = Color.white;

        _igniteFx.Stop();
        _chillFx.Stop();
        _shockFx.Stop();
    }

    public void ChillFxFor(float seconds)
    {
        _chillFx.Play();

        InvokeRepeating("ChillColorFX", 0, 0.3f);
        Invoke("Cancel—olorChange", seconds);
    }

    public void IgniteFxFor(float seconds)
    {
        _igniteFx.Play();

        InvokeRepeating("IgniteColorFX", 0, 0.3f);
        Invoke("Cancel—olorChange", seconds);
    }

    public void ShockFxFor(float seconds)
    {
        _shockFx.Play();

        InvokeRepeating("ShockColorFX", 0, 0.15f);
        Invoke("Cancel—olorChange", seconds);
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

    public void MakeTransparent(bool transparent)
    {
        if (transparent)
            _sr.color = Color.clear;
        else
            _sr.color = Color.white;
    }

    public void CreatHitFX(Transform target, bool critical)
    {
        float ZRotation = Random.Range(-90, 90);
        float xPosition = Random.Range(-.25f, .25f);
        float yPosition = Random.Range(-.25f, .25f);

        GameObject hitPrefab = _hitFx;

        if (critical)
            hitPrefab = _critHitFx;

        GameObject hitFX = Instantiate(hitPrefab, target.position + new Vector3(xPosition, yPosition), Quaternion.identity);

        if (!critical)
            hitFX.transform.Rotate(new Vector3(0, 0, ZRotation));
        else
            hitFX.transform.localScale = new Vector3(GetComponent<Entity>().FacingDir, 1, 1);

        Destroy(hitFX, 0.5f);
    }
}
