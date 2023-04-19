using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
   [SerializeField] private int _baseValue;

    public List<int> Modifiers;

    public int GetValue()
    {
        int finalValue = _baseValue;

        foreach (var modifier in Modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    } 

    public void SetDefaultValue(int value )
    {
        _baseValue = value;
    }

    public void AddModifier(int modifier)
    {
        Modifiers.Add(modifier);
    }

    public void RemoveModifier(int modifier)
    {
        Modifiers.RemoveAt(modifier);
    }
}
