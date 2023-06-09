using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy _enemy;
    private ItemDrop _myDropSystem;
    public Stat SoulsDropAmount;

    [Header("Level details")]
    [SerializeField] private int _level=1;

    [Range(0, 1)]
    [SerializeField] private float _percantageModifier=0.35f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        _enemy = GetComponent<Enemy>();
        _myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        Modify(Strength);
        Modify(Agility);
        Modify(Intelligence);
        Modify(Vitality);

        Modify(Damage);
        Modify(CritChance);
        Modify(CritPower);

        Modify(MaxHealth);
        Modify(Armor);
        Modify(Evasion);
        Modify(MagicResistance);

        Modify(FireDamage);
        Modify(IceDamage);
        Modify(LightingDamage);

        Modify(SoulsDropAmount);
    }

    private void Modify(Stat stat)
    {
        for(int i = 1; i < _level; i++)
        {
            float modifier = stat.GetValue() * _percantageModifier; 

            stat.AddModifier(Mathf.RoundToInt(modifier));
        } 
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        AudioManager.Instance.PlaySFX(43, transform);
    }

    protected override void Die()
    {
        base.Die();

        _enemy.Die();

        PlayerManager.Instance._currency += SoulsDropAmount.GetValue();
        _myDropSystem.GenerateDrop();

        Destroy(gameObject, 3f);
    }
}
 