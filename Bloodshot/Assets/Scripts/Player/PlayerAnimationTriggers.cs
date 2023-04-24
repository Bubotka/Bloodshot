using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.AttackCheck.position, player.AttackCheckRaduis);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats target = hit.GetComponent<EnemyStats>();

                if (target != null)
                    player.Stats.DoDamage(target);

                player.Stats.DoDamage(target);

                ItemDataEquipment weaponData = PlayerInventory.Instance.GetEquipment(EquipmentType.Weapon);

                if (weaponData != null)
                    weaponData.Effect(target.transform);
            }
        }
    }

    private void ThrowSword()
    {
        SkillManager.Instance.Sword.CreateSword();
    }
}
