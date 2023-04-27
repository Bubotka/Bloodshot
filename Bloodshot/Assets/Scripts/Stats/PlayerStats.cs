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
}
