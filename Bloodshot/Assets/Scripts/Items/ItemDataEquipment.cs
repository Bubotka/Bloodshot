using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor,
    Amulet,
    Flask  
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType EquipmentType;

    [Header ("Unique effect")]
    public float ItemCooldown;
    public ItemEffect[] ItemEffects;


    [Header("Major stats")]
    public int Strength;
    public int Agility;
    public int Intelligence;
    public int Vitality;

    [Header("Offensive stats")]
    public int Damage;
    public int CritChance;
    public int CritPower;

    [Header("Defensive stats")]
    public int Health;
    public int Armor;
    public int Evasion;
    public int MagicResistance;

    [Header("Magic stats")]
    public int FireDamage;
    public int IceDamage;
    public int LightingDamage;

    private int _descriptionLength;

    public void Effect(Transform enemyPosition)
    {
        foreach(var item in ItemEffects)
        {
            item.ExecuteEffect( enemyPosition);
        }
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.Strength.AddModifier(Strength);
        playerStats.Agility.AddModifier(Agility);
        playerStats.Intelligence.AddModifier(Intelligence);
        playerStats.Vitality.AddModifier(Vitality);

        playerStats.Damage.AddModifier(Damage);
        playerStats.CritChance.AddModifier(CritChance);
        playerStats.CritPower.AddModifier(CritPower);

        playerStats.MaxHealth.AddModifier(Health);
        playerStats.Armor.AddModifier(Armor);
        playerStats.Evasion.AddModifier(Evasion);
        playerStats.MagicResistance.AddModifier(MagicResistance);

        playerStats.FireDamage.AddModifier(FireDamage);
        playerStats.IceDamage.AddModifier(IceDamage);
        playerStats.LightingDamage.AddModifier(LightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        playerStats.Strength.RemoveModifier(Strength);
        playerStats.Agility.RemoveModifier(Agility);
        playerStats.Intelligence.RemoveModifier(Intelligence);
        playerStats.Vitality.RemoveModifier(Vitality);

        playerStats.Damage.RemoveModifier(Damage);
        playerStats.CritChance.RemoveModifier(CritChance);
        playerStats.CritPower.RemoveModifier(CritPower);

        playerStats.MaxHealth.RemoveModifier(Health);
        playerStats.Armor.RemoveModifier(Armor);
        playerStats.Evasion.RemoveModifier(Evasion);
        playerStats.MagicResistance.RemoveModifier(MagicResistance);

        playerStats.FireDamage.RemoveModifier(FireDamage);
        playerStats.IceDamage.RemoveModifier(IceDamage);
        playerStats.LightingDamage.RemoveModifier(LightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        _descriptionLength = 0;

        AddItemDescription(Strength, "Strength");
        AddItemDescription(Agility, "Agility");
        AddItemDescription(Intelligence, "Intelligence");
        AddItemDescription(Vitality, "Vitality");

        AddItemDescription(Damage, "Damage");
        AddItemDescription(CritChance, "CritChance");
        AddItemDescription(CritPower, "CritPower");

        AddItemDescription(Health, "Health");
        AddItemDescription(Evasion, "Evasion");
        AddItemDescription(Armor, "Armor");
        AddItemDescription(MagicResistance, "MagicResistance");

        AddItemDescription(FireDamage, "FireDamage");
        AddItemDescription(IceDamage, "IceDamage");
        AddItemDescription(LightingDamage, "LightingDamage");

        for (int i = 0; i < ItemEffects.Length; i++)
        {
            if (ItemEffects[i].EffectDescription.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: "+ItemEffects[i].EffectDescription);
                _descriptionLength++; 
            }
        }

        if (_descriptionLength < 5)
        {
            for (int i = 0; i < 5-_descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        return sb.ToString();
    }

    private void AddItemDescription(int value,string name)
    {
        if(value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();

            if (value > 0)
                sb.Append("+ "+value+" "+name);

            _descriptionLength++;
        }
    }
}
