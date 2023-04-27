using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff effect", menuName = "Data/Item effect/Buff effect")]
public class BuffEffect : ItemEffect
{
    [SerializeField] private StatType _buffType;
    [SerializeField] private int _buffAmount;
    [SerializeField] private float _buffDuration;

    private PlayerStats _stats;
    public override void ExecuteEffect(Transform enemyPosition)
    {
        _stats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        _stats.IncreaseStatBy(_buffAmount, _buffDuration, _stats.GetStat(_buffType));
    }
} 
   