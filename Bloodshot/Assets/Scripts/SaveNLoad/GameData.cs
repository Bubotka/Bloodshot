using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Currency;

    public SerializableDictionary<string, int> Inventory;
    public SerializableDictionary<string, bool> SkillTree;
    public List<string> EquipmentId;

    public SerializableDictionary<string, bool> Checkpoints;
    public string ClosestCheckpointId;

    public float LostCurrencyX;
    public float LostCurrencyY;
    public int LostCurrencyAmount;

    public GameData()
    {
        LostCurrencyX=0;
        LostCurrencyY=0;
        LostCurrencyAmount = 0;

        Currency = 0;
        SkillTree = new SerializableDictionary<string, bool>();
        Inventory = new SerializableDictionary<string, int>();
        EquipmentId = new List<string>();

        ClosestCheckpointId = string.Empty;
        Checkpoints = new SerializableDictionary<string, bool>();
    }
}
