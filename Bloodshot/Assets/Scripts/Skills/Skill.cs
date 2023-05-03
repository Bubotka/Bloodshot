using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float Cooldown;
    public float CooldownTimer;
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.Player;

        CheckUnlock();
    }

    protected virtual void Update()
    {
        CooldownTimer -= Time.deltaTime;
    }

    protected virtual void CheckUnlock()
    {
      
    }

    public virtual bool CanUseSkill()
    {
        if (CooldownTimer < 0)
        {
            UseSkill();
            CooldownTimer = Cooldown;
            return true; 
        }

        player.Fx.CreatePopUpText("Cooldown");

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
