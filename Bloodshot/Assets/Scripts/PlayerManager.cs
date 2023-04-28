using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Player Player;

    [SerializeField] private int _currency;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else   
            Instance = this;
    }

    public bool HaveEnoughMoney(int price)
    {
        if (price > _currency)
        {
            Debug.Log("Not enough money");
            return false;
        }

        _currency -= price;
        return true;
    } 

    public int GetCurrency() => _currency;
}
