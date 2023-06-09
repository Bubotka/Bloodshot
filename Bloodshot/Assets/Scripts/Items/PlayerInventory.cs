using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInventory : MonoBehaviour, ISaveManager
{
    public float FlaskCooldown { get; private set; }

    public static PlayerInventory Instance;

    public List<ItemData> StartingItems;

    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public Dictionary<ItemData, InventoryItem> InventoryDictionary = new Dictionary<ItemData, InventoryItem>();

    public List<InventoryItem> Equipment = new List<InventoryItem>();
    public Dictionary<ItemDataEquipment, InventoryItem> EquipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

    [Header("Inventory UI")]

    [SerializeField] private Transform _inventorySlotParent;
    [SerializeField] private Transform _equipmentSlotParent;
    [SerializeField] private Transform _statSlotParent;

    private ItemSlotUI[] _inventoryItemSlot;
    private EquipmentSlotUI[] _equipmentItemSlot;
    private StatSlotUI[] _statSlot;

    [Header("Items cooldown")]
    private float _lastTimeUsedFlask;
    private float _lastTimeUsedArmor;

    private float _armorCooldown;

    [Header("Data base")]
    public List<ItemData> ItemDataBase;
    public List<InventoryItem> LoadedItems;
    public List<ItemDataEquipment> LoadedEquipment;

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
        _equipmentItemSlot = _equipmentSlotParent.GetComponentsInChildren<EquipmentSlotUI>();
        _statSlot = _statSlotParent.GetComponentsInChildren<StatSlotUI>();
        AddStartingItems();
    }

    private void AddStartingItems()
    {
        foreach (ItemDataEquipment item in LoadedEquipment)
        {
            EquipItem(item);
        }

        if (LoadedItems.Count > 0)
        {
            foreach (InventoryItem item in LoadedItems)
            {
                for (int i = 0; i < item.StackSize; i++)
                {
                    AddItem(item.Data);
                }
            }

            return;
        }

        for (int i = 0; i < StartingItems.Count; i++)
        {
            if (StartingItems[i] != null)
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
        for (int i = 0; i < _equipmentItemSlot.Length; i++)
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

        for (int i = 0; i < Inventory.Count; i++)
        {
            _inventoryItemSlot[i].UpdateSlot(Inventory[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < _statSlot.Length; i++)
        {
            _statSlot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData item)
    {
        if (item.ItemType == ItemType.Equipment && CanAddItem())
            AddToInventory(item);

        UpdateSlotUI();
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
        if (InventoryDictionary.TryGetValue(item, out InventoryItem value))
        {
            if (value.StackSize <= 1)
            {
                Inventory.Remove(value);
                InventoryDictionary.Remove(item);
            }
            else
                value.RemoveStack();
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if (Inventory.Count >= _inventoryItemSlot.Length)
        {
            return false;
        }

        return true;
    }

    public List<InventoryItem> GetEquipmentList() => Equipment;

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

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > _lastTimeUsedFlask + FlaskCooldown;

        if (canUseFlask)
        {
            FlaskCooldown = currentFlask.ItemCooldown;
            currentFlask.Effect(null);
            _lastTimeUsedFlask = Time.time;
        }
        else
            Debug.Log("Flask on cooldown");
    }

    public bool CanUseArmor()
    {
        ItemDataEquipment currentArmor = GetEquipment(EquipmentType.Armor);

        if (Time.time > _lastTimeUsedArmor + _armorCooldown)
        {
            _armorCooldown = currentArmor.ItemCooldown;

            _lastTimeUsedArmor = Time.time;

            return true;
        }

        Debug.Log("Armor on cooldown");

        return false;
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, int> pair in data.Inventory)
        {
            foreach (var item in ItemDataBase)
            {
                if (item != null && item.ItemId == pair.Key)
                {
                    InventoryItem itemToLoad = new InventoryItem(item);
                    itemToLoad.StackSize = pair.Value;

                    LoadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in data.EquipmentId)
        {
            foreach (var item in ItemDataBase)
            {
                if (item != null && loadedItemId == item.ItemId)
                {
                    LoadedEquipment.Add(item as ItemDataEquipment);
                }
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.Inventory.Clear();
        data.EquipmentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in InventoryDictionary)
        {
            data.Inventory.Add(pair.Key.ItemId, pair.Value.StackSize);
        }

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> pair in EquipmentDictionary)
        {
            data.EquipmentId.Add(pair.Key.ItemId);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Fill up item data base")]
    private void FillUpItemDataBase() => ItemDataBase = new List<ItemData>(GetItemDataBase());

    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data/Equipment" });

        foreach (string SOName in assetNames)
        {
            var sOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(sOPath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif
}
