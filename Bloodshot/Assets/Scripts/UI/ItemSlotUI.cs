using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;

    public InventoryItem Item;

    public void UpdateSlot(InventoryItem newItem)
    {
        Item = newItem;

        _itemImage.color = Color.white;

        if (Item != null)
        {
            _itemImage.sprite = Item.Data.Icon;

            if (Item.StackSize > 1)
            {
                _itemText.text = Item.StackSize.ToString();
            }
            else
            {
                _itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        Item = null;

        _itemImage.sprite = null;
        _itemImage.color = Color.clear;
        _itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerInventory.Instance.RemoveItem(Item.Data);
            return;
        }

        if (Item.Data.ItemType == ItemType.Equipment)
            PlayerInventory.Instance.EquipItem(Item.Data);
    }
}
