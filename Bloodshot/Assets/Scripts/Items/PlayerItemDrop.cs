using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's drop")]
    [SerializeField] private float _chanceToLooseItems;
    [SerializeField] private float _chanceToLooseMaterials;

    public override void GenerateDrop()
    {
        PlayerInventory inventory = PlayerInventory.Instance;

        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLoose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if (Random.Range(0, 100) <= _chanceToLooseItems)
            {
                DropItem(item.Data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {

            inventory.UnequipItem(itemsToUnequip[i].Data as ItemDataEquipment);
        }
    }
}
