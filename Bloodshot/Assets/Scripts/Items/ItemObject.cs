using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private ItemData _itemData;

    private void SetupVisuals()
    {
        if (_itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = _itemData.Icon;
        gameObject.name = "Item object - " + _itemData.ItemName;
    }

    public void SetupItem(ItemData itemData, Vector2 velocity)
    {
        _itemData = itemData;
        _rb.velocity = velocity;

        SetupVisuals();
    }

    public void PickUpItem()
    {
        if (!PlayerInventory.Instance.CanAddItem() && _itemData.ItemType == ItemType.Equipment)
        {
            _rb.velocity = new Vector2(0, 7);
            return; 
        }

        AudioManager.Instance.PlaySFX(19, transform);
        PlayerInventory.Instance.AddItem(_itemData);
        Destroy(gameObject);
    }
}
