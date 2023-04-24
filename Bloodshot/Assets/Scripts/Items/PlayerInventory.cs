using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<ItemData> StartingItems;

    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public Dictionary<ItemData,InventoryItem> InventoryDictionary=new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> Stash = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> StashDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> Equipment = new List<InventoryItem>();
    public Dictionary<ItemDataEquipment, InventoryItem> EquipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

    [Header("Inventory UI")]

    [SerializeField] private Transform _inventorySlotParent;
    [SerializeField] private Transform _stashSlotParent;
    [SerializeField] private Transform _equipmentSlotParent;

    private ItemSlotUI[] _inventoryItemSlot;
    private ItemSlotUI[] _stashItemSlot;
    private EquipmentSlotUI[] _equipmentItemSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _inventoryItemSlot = _inventorySlotParent.GetComponentsInChildren<ItemSlotUI>();
        _stashItemSlot = _stashSlotParent.GetComponentsInChildren<ItemSlotUI>();
        _equipmentItemSlot = _equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {
        for (int i = 0; i < StartingItems.Count; i++)
        {
            AddItem(StartingItems[i]);
        }
    }

    public void EquipItem(ItemData item)
    {
        ItemDataEquipment newEuipment = item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEuipment);

        ItemDataEquipment oldEquipment = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> itemD in EquipmentDictionary)
        {
            if (itemD.Key.EquipmentType == newEuipment.EquipmentType)
                oldEquipment = itemD.Key;
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        Equipment.Add(newItem);
        EquipmentDictionary.Add(newEuipment, newItem);
        newEuipment.AddModifiers();

        RemoveItem(item);


        UpdateSlotUI();
    }

    public void UnequipItem(ItemDataEquipment itemToRemove)
    {
        if (EquipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            Equipment.Remove(value);
            EquipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {
        for(int i=0;i< _equipmentItemSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemDataEquipment, InventoryItem> itemD in EquipmentDictionary)
            {
                if (itemD.Key.EquipmentType == _equipmentItemSlot[i].SlotType)
                    _equipmentItemSlot[i].UpdateSlot(itemD.Value);
            }
        }

        for (int i = 0; i < _inventoryItemSlot.Length; i++)
        {
            _inventoryItemSlot[i].CleanUpSlot();
        }
        for (int i = 0; i < _stashItemSlot.Length; i++)
        {
            _stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i < Inventory.Count; i++)
        {
            _inventoryItemSlot[i].UpdateSlot(Inventory[i]);
        }
        for (int i = 0; i < Stash.Count; i++)
        {
            _stashItemSlot[i].UpdateSlot(Stash[i]);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.ItemType == ItemType.Equipment)
            AddToInventory(item);   
        else if(item.ItemType == ItemType.Material)
            AddToStash(item);

        UpdateSlotUI();
    }

    private void AddToStash(ItemData item)
    {
        if (StashDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            Stash.Add(newItem);
            StashDictionary.Add(item, newItem);
        }
    }

    private void AddToInventory(ItemData item)
    {
        if (InventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            Inventory.Add(newItem);
            InventoryDictionary.Add(item, newItem);
        }
    }

    public void RemoveItem(ItemData item)
    {
        if(InventoryDictionary.TryGetValue(item,out InventoryItem value))
        {
            if (value.StackSize <= 1)
            {
                Inventory.Remove(value);
                InventoryDictionary.Remove(item);
            }
            else
                value.RemoveStack();
        }

        else if (StashDictionary.TryGetValue(item, out InventoryItem stashvalue))
        {
            if (stashvalue.StackSize <= 1)
            {
                Stash.Remove(stashvalue);
                StashDictionary.Remove(item);
            }
            else
               stashvalue.RemoveStack();
        }

        UpdateSlotUI();
    }

    public List<InventoryItem> GetEquipmentList() => Equipment;

    public List<InventoryItem> GetStashList() => Stash;

    public ItemDataEquipment GetEquipment(EquipmentType type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> itemD in EquipmentDictionary)
        {
            if (itemD.Key.EquipmentType == type)
                equipedItem = itemD.Key;
        }

        return equipedItem;
    }
}
