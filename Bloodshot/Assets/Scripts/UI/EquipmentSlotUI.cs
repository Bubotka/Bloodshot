using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlotUI : ItemSlotUI
{
    public EquipmentType SlotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot- " + SlotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Item == null || Item.Data == null)
            return;

        PlayerInventory.Instance.UnequipItem(Item.Data as ItemDataEquipment);
        PlayerInventory.Instance.AddItem(Item.Data as ItemDataEquipment);
        CleanUpSlot();
    }
} 
