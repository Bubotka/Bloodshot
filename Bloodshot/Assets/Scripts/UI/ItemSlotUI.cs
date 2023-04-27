using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _itemText;

    public InventoryItem Item;
    private UI _ui => GetComponentInParent<UI>();

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
        if (Item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerInventory.Instance.RemoveItem(Item.Data);
            return;
        }

        if (Item.Data.ItemType == ItemType.Equipment)
            PlayerInventory.Instance.EquipItem(Item.Data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item == null)
            return;

        Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if (mousePosition.x > 300)
            xOffset = -100;
        else
            xOffset = 100;

        if (mousePosition.y > 100)
            yOffset = 300;
        else
            yOffset = -75;

        _ui.ItemToolTip.ShowToolTip(Item.Data as ItemDataEquipment);
        _ui.ItemToolTip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item == null)
            return;

        _ui.ItemToolTip.HideToolTip();
    }
} 
