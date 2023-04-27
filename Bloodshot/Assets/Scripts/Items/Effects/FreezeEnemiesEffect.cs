using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies effect", menuName = "Data/Item effect/Freeze enemies")]
public class FreezeEnemiesEffect : ItemEffect
{
    [SerializeField] private float _duration;

    public override void ExecuteEffect(Transform transform)
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        if (playerStats.CurrentHealth > playerStats.GetMaxHealthValue() * 0.15f)
            return;

        if (!PlayerInventory.Instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 5);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(_duration);
        }
    }
}
