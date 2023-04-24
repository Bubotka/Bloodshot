using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder strike effect", menuName = "Data/Item effect/ThunderStrike")]
public class ThunderStrikeEffect : ItemEffect
{
    [SerializeField] private GameObject _thunderStrikePrefab;

    public override void ExecuteEffect(Transform enemyPosition)
    {
        GameObject newThunderStrike = Instantiate(_thunderStrikePrefab, enemyPosition.position,Quaternion.identity);

        Destroy(newThunderStrike, 1f);
    }
}
