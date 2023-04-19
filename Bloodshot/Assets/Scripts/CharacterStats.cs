using System;
using System.Data.Common;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
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

    private float _ignitedTimer;
    private float _chilledTimer;
    private float _shockedTimer;

    private float _ignitedDamageCooldown = 0.3f;
    private float _ignitedDamageTimer;
    private int _igniteDamage;


    public int CurrentHealth;

    public event Action HealthChanged;

    protected virtual void Start()
    {
        CritPower.SetDefaultValue(150);
        CurrentHealth = GetMaxHealthValue();
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

        if (_ignitedDamageTimer < 0 && IsIgnited)
        {
            DecreaseHealthBy(_igniteDamage);

            if (CurrentHealth < 0)
                Die(); 

            _ignitedDamageTimer = _ignitedDamageCooldown;
        }
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        if (TargetCanAvoidAttack(targetStats))
            return;

        int totalDamage = Damage.GetValue() + Strength.GetValue();

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(targetStats, totalDamage);

        //targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(targetStats);
    }

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
            targetStats.SetupIgniteDamage(Mathf.RoundToInt(fireDamage * 0.2f));

        targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
    }

    private static int CheckTargetResistance(CharacterStats targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= targetStats.MagicResistance.GetValue() + (targetStats.Intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAliments(bool ignite, bool chill, bool shock)
    {
        if (IsIgnited || IsChilled || IsShocked)
            return;

        if (ignite)
        {
            IsIgnited = ignite;
            _ignitedTimer = 2;
        }

        if (chill)
        {
            IsChilled = chill;
            _chilledTimer = 2;
        }

        if (shock)
        {
            IsShocked = shock;
            _shockedTimer = 2;
        }    
    }

    public virtual void TakeDamage(int damage)
    {
        DecreaseHealthBy(damage);

        if (CurrentHealth < 0)
            Die();
    }

    protected virtual void DecreaseHealthBy(int damage)
    {
        CurrentHealth -= damage;

        if (HealthChanged != null)
            HealthChanged();
    }

    public void SetupIgniteDamage(int damage) => _igniteDamage = damage;

    protected virtual void Die()
    {

    }

    private bool CanCrit()
    {
        int totalCritChance = CritChance.GetValue() + Agility.GetValue();

        if (UnityEngine.Random.Range(0, 100) < totalCritChance)
        {
            return true;
        }

        return false;
    }

    private bool TargetCanAvoidAttack(CharacterStats targetStats)
    {
        int totalEvasion = targetStats.Evasion.GetValue() + targetStats.Agility.GetValue();

        if (IsShocked)
            totalEvasion += 20;

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }

        return false;
    }

    private int CheckTargetArmor(CharacterStats targetStats, int totalDamage)
    {
        if (targetStats.IsChilled)
            totalDamage -= Mathf.RoundToInt(targetStats.Armor.GetValue() * 0.8f);
        else
            totalDamage -= targetStats.Armor.GetValue();

        totalDamage -= Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private int CalculateCriticalDamage(int damage)
    {
        float totalCritPower = (CritPower.GetValue() + Strength.GetValue()) * 0.1f;

        float critDamage = damage * totalCritPower;

        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue() => MaxHealth.GetValue() + (Vitality.GetValue() * 5);
}
