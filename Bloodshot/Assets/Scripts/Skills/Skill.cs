using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.Player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true; 
        }
        Debug.Log("Cooldown");
        return false; 
    }

    public virtual void UseSkill()
    {
        Debug.Log("Skill is used");
    }

    protected virtual Transform FindClosestEnemy(Transform checkTransform)
    {
        Collider2D[] coliders = Physics2D.OverlapCircleAll(checkTransform.position,12);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null; 

        foreach (var hit in coliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(checkTransform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }
        return closestEnemy;
    }
}
