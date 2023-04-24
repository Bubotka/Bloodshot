using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject _myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            if (collision.GetComponent<CharacterStats>().IsDead)
                return;

            Debug.Log("PickUp");
            _myItemObject.PickUpItem();
        }
    }
}
