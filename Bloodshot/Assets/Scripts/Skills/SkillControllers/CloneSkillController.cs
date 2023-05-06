using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    [SerializeField] private Transform _attackCheck;
    [SerializeField] private float _attackCheckRadius = 0.4f;
    [SerializeField] private float _colorLoosingSpeed;

    private float _attackMultiplier;
    private Player _player;
    private SpriteRenderer _sr;
    private float _cloneTimer;
    private Transform _closestEnemy;
    private bool _canDuplicateClone;
    private int _facingDir = 1;
    private float _chanceToDuplicate;

    private Animator _anim;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _cloneTimer -= Time.deltaTime;

        if (_cloneTimer < 0)
        {
            _sr.color = new Color(1, 1, 1, _sr.color.a - (Time.deltaTime * _colorLoosingSpeed));

            if (_sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, 
        Vector3 offset, Transform closestEnemy, bool canDuplicate, float chanceToDuplicate, Player player, float attackMultiplier)
    {
        if (canAttack)
            _anim.SetInteger("AttackNumber", Random.Range(1, 3));

        transform.position = newTransform.position + offset;
        _cloneTimer = cloneDuration;
        _canDuplicateClone = canDuplicate;
        _closestEnemy = closestEnemy;
        _chanceToDuplicate = chanceToDuplicate;
        _player = player;
        _attackMultiplier = attackMultiplier;
        FaceClosestTarget();
        AudioManager.Instance.PlaySFX(3, transform);
    }

    private void AnimationTrigger()
    { 
        _cloneTimer = -0.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_attackCheck.position, _attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = _player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                playerStats.CloneDoDamage(enemyStats, _attackMultiplier);

                if (_player.Skill.Clone.CanApplyOnHitEffect)
                {

                    ItemDataEquipment weaponData = PlayerInventory.Instance.GetEquipment(EquipmentType.Weapon);

                    if (weaponData != null)
                        weaponData.Effect(hit.transform);
                }

                if (_canDuplicateClone)
                {
                    if (Random.Range(0, 100) < _chanceToDuplicate)
                    {
                        SkillManager.Instance.Clone.CreateClone(hit.transform, new Vector3(.5f * _facingDir, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if (_closestEnemy != null)
        {
            Debug.Log(_closestEnemy.name);
            if (transform.position.x > _closestEnemy.position.x)
            {
                _facingDir = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
