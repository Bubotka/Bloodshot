using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBoss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.Instance.PlaySFX(48,null);
    }
}
