using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LostCurrencyController : MonoBehaviour
{
    public int Currency;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            PlayerManager.Instance._currency += Currency;
            Destroy(gameObject);
        }
    }
}
