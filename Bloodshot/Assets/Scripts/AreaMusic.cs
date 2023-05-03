using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaMusic : MonoBehaviour
{
    [SerializeField] private int _areaMusicIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.Instance.PlayBgM(_areaMusicIndex);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            AudioManager.Instance.StopBGMWithTime(_areaMusicIndex);
    }
}
