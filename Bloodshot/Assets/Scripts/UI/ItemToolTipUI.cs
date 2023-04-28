using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemToolTipUI : ToolTipUI
{
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _itemTypeText;
    [SerializeField] private TextMeshProUGUI _itemDescription;

    public void ShowToolTip(ItemDataEquipment item)
    {
        if (item == null)
            return;

        _itemNameText.text = item.ItemName;
        _itemTypeText.text = item.EquipmentType.ToString();
        _itemDescription.text = item.GetDescription();

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false); 
}
