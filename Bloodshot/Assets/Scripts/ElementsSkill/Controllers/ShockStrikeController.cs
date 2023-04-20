using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrikeController : MonoBehaviour
{
    [SerializeField] private CharacterStats _targetStats;
    [SerializeField] private float _speed;
    private int _damage;

    private Animator _anim;
    private bool _triggered;

    private void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void Setup(int damage, CharacterStats targetStats)
    {
        _damage = damage;
        _targetStats = targetStats;
    }

    private void Update()
    {
        if (!_targetStats)
            return;

        if (_triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _targetStats.transform.position, _speed * Time.deltaTime);
        transform.right = transform.position - _targetStats.transform.position;

        if(Vector2.Distance(transform.position, _targetStats.transform.position) < 0.05f)
        {
            _anim.transform.localRotation = Quaternion.identity;
            _anim.transform.localPosition = new Vector3(0, 0.25f);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);

            Invoke("DamageAndSelfDestroy", 0.2f);
            _triggered = true;
            _anim.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        _targetStats.ApplyShock(true);
        _targetStats.TakeDamage(_damage);

        Destroy(gameObject, 0.4f);    
    }
}
