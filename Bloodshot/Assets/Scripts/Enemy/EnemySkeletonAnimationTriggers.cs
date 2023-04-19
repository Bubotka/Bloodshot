using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton _enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        _enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemy.AttackCheck.position, _enemy.AttackCheckRaduis);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<PlayerStats>();

                _enemy.Stats.DoDamage(target);
            }
        }
    }

    private void OpenCounterWindow() => _enemy.OpenCounterAttackWindow();

    private void CloseCounterWindow() => _enemy.CloseCounterAttackWindow();
}
