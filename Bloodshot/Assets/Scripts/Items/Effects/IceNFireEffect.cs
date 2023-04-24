using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ice and fire effect", menuName = "Data/Item effect/Ice and fire")]
public class IceNFireEffect : ItemEffect
{
    [SerializeField] private GameObject _iceNFirePrefab;
    [SerializeField] private float _xVelocity;
    public override void ExecuteEffect(Transform respawnPosition)
    {
        Player player = PlayerManager.Instance.Player;

        bool thirdAttack = player.PrimaryAttackState.ComboCounter == 2;

        if (thirdAttack)
        {
            GameObject newIceNFire = Instantiate(_iceNFirePrefab, respawnPosition.position, player.transform.rotation);

            newIceNFire.GetComponent<Rigidbody2D>().velocity = new Vector2(_xVelocity * player.FacingDir, 0);

            Destroy(newIceNFire, 8);
        }


    }
}

