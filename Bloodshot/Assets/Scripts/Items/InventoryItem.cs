using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem 
{
    public ItemData Data;
    public int StackSize;

    public InventoryItem (ItemData newItemData)
    {
        Data = newItemData;
        AddStack();
    }

    public void AddStack() => StackSize++;
    public void RemoveStack() => StackSize--;
}
