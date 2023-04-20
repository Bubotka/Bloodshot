using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] private float _crystalDuration;
    [SerializeField] private GameObject _crystalPrefab;
    private GameObject _currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool _cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool _canExplode;
    [SerializeField] private float _maxSizeCrystalGrow;
    [SerializeField] private float _growSpeed;

    [Header("Moving crystal")]
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;

    [Header("Multi crystal")]
    [SerializeField] private bool _canUseMultiStacks;
    [SerializeField] private int _amountOfStacks;
    [SerializeField] private float _multiStackCooldown;
    [SerializeField] private float _useTimeWindow;
    [SerializeField] private List<GameObject> _crystalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;

        if (_currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {
            if (_canMoveToEnemy)
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPos;

            if (_cloneInsteadOfCrystal)
            {
                SkillManager.Instance.Clone.CreateClone(_currentCrystal.transform, Vector3.zero);
                Destroy(gameObject);
            }
            else
            {
                _currentCrystal.GetComponent<CrystalSkillController>()?.FinishCrystal();

            }
        }
    }

    public void CreateCrystal()
    {
        _currentCrystal = Instantiate(_crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController currentCrystalScript = _currentCrystal.GetComponent<CrystalSkillController>();

        currentCrystalScript.SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, _growSpeed,
            _maxSizeCrystalGrow, FindClosestEnemy(_currentCrystal.transform), player);

    }

    public void CurrentCrystalChooseRandomTarget() => _currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();

    private bool CanUseMultiCrystal()
    {
        if (_canUseMultiStacks)
        {
            if (_crystalLeft.Count > 0)
            {
                if (_crystalLeft.Count == _amountOfStacks)
                    Invoke("ResetAbility", _useTimeWindow);

                cooldown = 0;

                GameObject crystalToSpawn = _crystalLeft[_crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                _crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy,
                    _moveSpeed, _growSpeed, _maxSizeCrystalGrow, FindClosestEnemy(newCrystal.transform), player);

                if (_crystalLeft.Count <= 0)
                {
                    cooldown = _multiStackCooldown;
                    RefilCrystal();
                }

                return true;
            }

        }

        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = _amountOfStacks - _crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            _crystalLeft.Add(_crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;

        cooldownTimer = _multiStackCooldown;
        RefilCrystal();
    }
}
