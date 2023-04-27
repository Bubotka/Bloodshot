using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/Heal effect")]
public class HealEffect : ItemEffect
{
    [Range(0, 1f)]
    [SerializeField] private float _healPercent;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        int healAmount =Mathf.RoundToInt(playerStats.GetMaxHealthValue() * _healPercent);

        playerStats.IncreaseHealthBy(healAmount);
    } 
}
