using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackholeSkill : Skill
{
    [SerializeField] private SkillTreeSlotUI _blackHoleUnlockButton;
    [SerializeField] private int _amountOfAttacks;
    [SerializeField] private float _cloneCooldown;
    [SerializeField] private float _blackHoleDuration;
    [Space]
    [SerializeField] private GameObject _blacholePrefab;
    [SerializeField] private float _maxSize;
    [SerializeField] private float _growSpeed;
    [SerializeField] private float _shrinkSpeed;

    public bool BlackHoleUnlocked { get; private set; }

    private BlackholeSkillController _currentBlackhole;

    private void UnlockBlackHole()
    {
        if (_blackHoleUnlockButton.Unlocked)
            BlackHoleUnlocked = true;
    }

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(_blacholePrefab,player.transform. position, Quaternion.identity);

        _currentBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();

        _currentBlackhole.SetupBlackHole(_maxSize,_growSpeed,_shrinkSpeed,_amountOfAttacks,_cloneCooldown,_blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        _blackHoleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBlackHole);
    }

    public bool SkillCompleted()
    {
        if (!_currentBlackhole) 
            return false;

        if (_currentBlackhole.PlayerCanExitState)
        {
            _currentBlackhole = null;
            return true;
        }

        return false;
    }

    public float GetBlackHoleRadius()
    {
        return _maxSize / 2;
    }
}
