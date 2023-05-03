using System;
using System.Collections;
using UnityEngine;

public enum StatType
{
    Strength,
    Agility,
    Intelegence,
    Vitality,
    Damage,
    CritChance,
    CritPower,
    Health,
    Armor,
    Evasion,
    MagicRes,
    FireDamage,
    IceDamage,
    LightingDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX _fx;

    [Header("Major stats")]
    public Stat Strength;
    public Stat Agility;
    public Stat Intelligence;
    public Stat Vitality;

    [Header("Offensive stats")]
    public Stat Damage;
    public Stat CritChance;
    public Stat CritPower;

    [Header("Defensive stats")]
    public Stat MaxHealth;
    public Stat Armor;
    public Stat Evasion;
    public Stat MagicResistance;

    [Header("Magic stats")]
    public Stat FireDamage;
    public Stat IceDamage;
    public Stat LightingDamage;

    public bool IsIgnited;
    public bool IsChilled;
    public bool IsShocked;

    [SerializeField] private float _alimentsDuration = 4;
    private float _ignitedTimer;
    private float _chilledTimer;
    private float _shockedTimer;

    [SerializeField] private GameObject _shockStrikePrefab;

    private float _ignitedDamageCooldown = 0.3f;
    private float _ignitedDamageTimer;
    private int _igniteDamage;
    private int _shockDamage;

    public int CurrentHealth;

    public event Action HealthChanged;

    public bool IsDead { get; private set; }

    public bool _isInvicible { get; private set; }
    private bool _isVulnerable;

    protected virtual void Start()
    {
        CritPower.SetDefaultValue(150);
        CurrentHealth = GetMaxHealthValue();

        _fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        _ignitedTimer -= Time.deltaTime;
        _chilledTimer -= Time.deltaTime;
        _shockedTimer -= Time.deltaTime;

        _ignitedDamageTimer -= Time.deltaTime;

        if (_ignitedTimer < 0)
            IsIgnited = false;

        if (_chilledTimer < 0)
            IsChilled = false;

        if (_shockedTimer < 0)
            IsShocked = false;

        if (IsIgnited)
            ApplyIgniteDamage();
    }
    public void MakeInvincible(bool invincible) => _isInvicible = invincible;

    public void MakeVulnerableFor(float duration)=> StartCoroutine(VelnerableCoroutine(duration));

    private IEnumerator VelnerableCoroutine(float duration)
    {
        _isVulnerable = true;

        yield return new WaitForSeconds(duration);

        _isVulnerable = false;
    }

    public virtual void IncreaseStatBy(int modifier, float duration, Stat statToModify)
    {
        StartCoroutine(StatModCoroutine(modifier, duration, statToModify));
    }

    private IEnumerator StatModCoroutine(int modifier, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifier);

        yield return new WaitForSeconds(duration); 

        statToModify.RemoveModifier(modifier);
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        bool criticalStrike = false;

        if (TargetCanAvoidAttack(targetStats))
            return;

        targetStats.GetComponent<Entity>().SetupKnockbackDir(transform);

        int totalDamage = Damage.GetValue() + Strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
            criticalStrike = true;
        }

        _fx.CreatHitFX(targetStats.transform,criticalStrike);

        totalDamage = CheckTargetArmor(targetStats, totalDamage);

        targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(targetStats);
    }

    public virtual void TakeDamage(int damage)
    {
        if (_isInvicible)
            return;

        DecreaseHealthBy(damage);

        GetComponent<Entity>().DamageImpact();
        _fx.StartCoroutine("FlashFX");

        if (CurrentHealth < 0 && !IsDead)
            Die();
    }

    public virtual void IncreaseHealthBy(int amount)
    {
        CurrentHealth += amount;

        if (CurrentHealth > GetMaxHealthValue())
            CurrentHealth = GetMaxHealthValue();

        if (HealthChanged != null)
            HealthChanged();
    }

    protected virtual void DecreaseHealthBy(int damage)
    {
        if (_isVulnerable)
            damage = Mathf.RoundToInt(damage * 1.1f);

        CurrentHealth -= damage;

        if (damage > 0)
            _fx.CreatePopUpText(damage.ToString());

        if (HealthChanged != null)
            HealthChanged();
    }

    protected virtual void Die() => IsDead = true;

    #region Magical damage and elements

    public virtual void DoMagicalDamage(CharacterStats targetStats)
    {
        int fireDamage = FireDamage.GetValue();
        int iceDamage = IceDamage.GetValue();
        int lightingDamage = LightingDamage.GetValue();

        int totalMagicalDamage = fireDamage + iceDamage + lightingDamage + Intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(targetStats, totalMagicalDamage);
        targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(fireDamage, iceDamage, lightingDamage) <= 0)
            return;

        AttemptToApplyElements(targetStats, fireDamage, iceDamage, lightingDamage);
    }

    private void AttemptToApplyElements(CharacterStats targetStats, int fireDamage, int iceDamage, int lightingDamage)
    {
        bool canApplyIgnite = fireDamage > iceDamage && fireDamage > lightingDamage;
        bool canApplyChill = iceDamage > fireDamage && iceDamage > lightingDamage;
        bool canApplyShock = lightingDamage > iceDamage && lightingDamage > fireDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (UnityEngine.Random.value < 0.4f && fireDamage > 0)
            {
                canApplyIgnite = true;
                targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < 0.45f && iceDamage > 0)
            {
                canApplyChill = true;
                targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            if (UnityEngine.Random.value < 0.5f && lightingDamage > 0)
            {
                canApplyShock = true;
                targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
        }

        if (canApplyIgnite)
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.1f));

        if (canApplyShock)
            targetStats.SetupsShockDamage(Mathf.RoundToInt(lightingDamage * 0.2f));

        targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    public void ApplyAliments(bool ignite, bool chill, bool shock)
    {
        bool canApplyIgnite = !IsIgnited && !IsChilled && !IsShocked;
        bool canApplyChill = !IsIgnited && !IsChilled && !IsShocked;
        bool canApplyShock = !IsIgnited && !IsChilled;

        if (ignite && canApplyIgnite)
        {
            IsIgnited = ignite;
            _ignitedTimer = _alimentsDuration;

            _fx.IgniteFxFor(_alimentsDuration);
        }

        if (chill && canApplyChill)
        {
            IsChilled = chill;
            _chilledTimer = _alimentsDuration;

            float slowPercentage = 0.2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, _alimentsDuration);
            _fx.ChillFxFor(_alimentsDuration);
        }

        if (shock && canApplyShock)
        {
            if (!IsShocked)
            {
                ApplyShock(shock);
            }
            else
            {
                if (GetComponent<Player>() != null)
                    return;

                HitShockStrike();
            }
        }
    }

    public void ApplyShock(bool shock)
    {
        if (IsShocked)
            return;

        IsShocked = shock;
        _shockedTimer = _alimentsDuration;

        _fx.ShockFxFor(_alimentsDuration);
    }

    private void ApplyIgniteDamage()
    {
        if (_ignitedDamageTimer < 0)
        {
            DecreaseHealthBy(_igniteDamage);

            if (CurrentHealth <= 0 && !IsDead)
                Die();

            _ignitedDamageTimer = _ignitedDamageCooldown;
        }
    }

    private void HitShockStrike()
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll(transform.position, 12);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in coliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(_shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrikeController>().Setup(_shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupsShockDamage(int damage) => _shockDamage = damage;

    public void SetupIgniteDamage(int damage) => _igniteDamage = damage;

    #endregion

    #region Stats calculations

    protected bool CanCrit()
    {
        int totalCritChance = CritChance.GetValue() + Agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }

        return false;
    }

    public virtual void OnEvasion()
    {

    }

    protected bool TargetCanAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.Evasion.GetValue() + targetStats.Agility.GetValue();

        if (IsShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            targetStats.OnEvasion();
            return true;
        }

        return false;
    }

    protected int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
    {
        if (targetStats.IsChilled)
            totalDamage -= Mathf.RoundToInt(targetStats.Armor.GetValue() * 0.8f);
        else
            totalDamage -= targetStats.Armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    protected int CalculateCriticalDamage(int damage)
    {
        float totalCritPower = (CritPower.GetValue() + Strength.GetValue()) * 0.01f;

        float critDamage = damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    private int CheckTargetResistance(CharacterStats targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= targetStats.MagicResistance.GetValue() + (targetStats.Intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public int GetMaxHealthValue() => MaxHealth.GetValue() + (Vitality.GetValue() * 5);

    #endregion

    public Stat GetStat(StatType statType)
    {
        if (statType == StatType.Strength) return Strength;
        else if (statType == StatType.Agility) return Agility;
        else if (statType == StatType.Intelegence) return Intelligence;
        else if (statType == StatType.Vitality) return Vitality;
        else if (statType == StatType.Damage) return Damage;
        else if (statType == StatType.CritChance) return CritChance;
        else if (statType == StatType.CritPower) return CritPower;
        else if (statType == StatType.Health) return MaxHealth;
        else if (statType == StatType.Armor) return Armor;
        else if (statType == StatType.Evasion) return Evasion;
        else if (statType == StatType.MagicRes) return MagicResistance;
        else if (statType == StatType.FireDamage) return FireDamage;
        else if (statType == StatType.IceDamage) return IceDamage;
        else if (statType == StatType.LightingDamage) return LightingDamage;

        return null;
    }

    public void KillEntity()
    {
        if (!IsDead)
            Die();
    }
}
