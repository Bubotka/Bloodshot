using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int _possibleDropOfItems;
    [SerializeField] private ItemData[] _possibleDrop;
    private List<ItemData> _dropList = new List<ItemData>();

    [SerializeField] private GameObject _dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < _possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= _possibleDrop[i].DropChance)
                _dropList.Add(_possibleDrop[i]);
        }

        for(int i = 0; i < _possibleDropOfItems; i++)
        {
            if (_dropList.Count == 0)
                return;

            ItemData randomItem = _dropList[Random.Range(0, _dropList.Count)];  

            _dropList.Remove(randomItem);
            DropItem(randomItem);
        } 
    }

    protected void DropItem(ItemData itemData)
    {
        GameObject newDrop = Instantiate(_dropPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-6, 6), Random.Range(10, 15));

        newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
    }
}
 