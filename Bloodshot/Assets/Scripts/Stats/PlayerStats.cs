using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player _player;

    protected override void Start()
    {
        base.Start();

        _player = GetComponent<Player>();
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
        _player.Die();

        GetComponent<PlayerItemDrop>()?.GenerateDrop();
    }

    protected override void DecreaseHealthBy(int damage)
    {
        base.DecreaseHealthBy(damage);

        ItemDataEquipment currentArmor = PlayerInventory.Instance.GetEquipment(EquipmentType.Armor);

        if (currentArmor != null)
            currentArmor.Effect(_player.transform);
    }

    public override void OnEvasion()
    {
        _player.Skill.Dodge.CreateMiragenOnDodge();
    }

    public void CloneDoDamage(CharacterStats targetStats, float multiplier)
    {
        if (TargetCanAvoidAttack(targetStats))
            return;

        int totalDamage = Damage.GetValue() + Strength.GetValue();

        if (multiplier > 0)
            totalDamage = Mathf.RoundToInt(totalDamage + multiplier);

        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage); 
        }

        totalDamage = CheckTargetArmor(targetStats, totalDamage);
         
        targetStats.TakeDamage(totalDamage);

        DoMagicalDamage(targetStats);
    }
}
