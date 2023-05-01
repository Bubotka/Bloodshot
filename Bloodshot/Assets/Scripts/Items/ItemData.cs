using System.Text;
using UnityEditor;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName ="New Item Data",menuName ="Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;
    public string ItemName;
    public Sprite Icon;
    public string ItemId;

    [Range(0,100)]
    public float DropChance;

    protected StringBuilder sb = new StringBuilder();

    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        ItemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }
}
