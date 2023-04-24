using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class BlackholeSkillController : MonoBehaviour
{
    public bool PlayerCanExitState { get; private set; }

    [SerializeField] private GameObject _hotKeyPrefab;
    [SerializeField] private List<KeyCode> _keyCodeList;

    private float _maxSize;
    private float _growSpeed;
    private float _shrinkSpeed;
    private float _blackholeTimer;

    private bool _canGrow = true;
    private bool _canShrink;
    private bool _playerCanDisapear = true;

    private int _amountOfAttacks = 4;
    private float _cloneAttackCooldown = 0.3f;
    private float _cloneAttackTimer;
    private bool _cloneAttackReleased;
    private bool _canCreateHotKeys = true;

    private List<Transform> _targets = new List<Transform>();
    private List<GameObject> _createdHotKey = new List<GameObject>();

    public void SetupBlackHole(float maxSize, float growSpeed, float shrinkSpeed, int amountOfAttacks, float cloneAttackCooldown, float blackholeTimer)
    {
        _maxSize = maxSize;
        _growSpeed = growSpeed;
        _shrinkSpeed = shrinkSpeed;
        _amountOfAttacks = amountOfAttacks;
        _cloneAttackCooldown = cloneAttackCooldown;
        _blackholeTimer = blackholeTimer;

        if (SkillManager.Instance.Clone.CrystallInsteadOfClone)
            _playerCanDisapear = false;
    }

    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime;
        _blackholeTimer -= Time.deltaTime;

        if (_blackholeTimer < 0)
        {
            _blackholeTimer = Mathf.Infinity;

            if (_targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackHole();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (_canGrow && !_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        }

        if (_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (_targets.Count <= 0)
            return;

        DestroyHotKeys();
        _cloneAttackReleased = true;
        _canCreateHotKeys = false;

        if (_playerCanDisapear)
        {
            _playerCanDisapear = false;
            PlayerManager.Instance.Player.Fx.MakeTransparent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (_cloneAttackTimer < 0 && _cloneAttackReleased && _amountOfAttacks > 0)
        {
            _cloneAttackTimer = _cloneAttackCooldown;

            int randomIndex = Random.Range(0, _targets.Count);

            float xOffset;

            if (Random.Range(0, 20) > 10)
                xOffset = 1;
            else
                xOffset = -1;

            if (SkillManager.Instance.Clone.CrystallInsteadOfClone)
            {
                SkillManager.Instance.Crystal.CreateCrystal();
                SkillManager.Instance.Crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
            SkillManager.Instance.Clone.CreateClone(_targets[randomIndex], new Vector3(xOffset, 0));
            }
            _amountOfAttacks--;

            if (_amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHole", 1f);
            }
        }
    }

    private void FinishBlackHole()
    {
        DestroyHotKeys();
        PlayerCanExitState = true;
        _canShrink = true;
        _cloneAttackReleased = false;
    }

    private void DestroyHotKeys()
    {
        if (_createdHotKey.Count <= 0)
            return;
        for (int i = 0; i < _createdHotKey.Count; i++)
        {
            Destroy(_createdHotKey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }
    
    private void CreateHotKey(Collider2D collision)
    {
        if (_keyCodeList.Count < 0)
        {
            Debug.Log("Not enough hot keys");
            return;
        }

        if (!_canCreateHotKeys)
            return;

        GameObject newHotKey = Instantiate(_hotKeyPrefab, collision.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        _createdHotKey.Add(newHotKey);

        KeyCode choosenKey = _keyCodeList[Random.Range(0, _keyCodeList.Count)];
        _keyCodeList.Remove(choosenKey);

        BlackholeHotKeyController newHotKeyScript = newHotKey.GetComponent<BlackholeHotKeyController>();

        newHotKeyScript.SetupHotKey(choosenKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform enemyTransform) => _targets.Add(enemyTransform);
}
