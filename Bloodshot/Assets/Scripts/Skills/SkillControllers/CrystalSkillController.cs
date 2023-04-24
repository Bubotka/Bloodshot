using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{

    private Animator _anim => GetComponent<Animator>();
    private CircleCollider2D _cd => GetComponent<CircleCollider2D>();
    private Player _player;

    private float _crystalExistTimer;
    private bool _canGrow;
    private float _growSpeed;

    private bool _canExplode;
    private bool _canMove;
    private float _moveSpeed;
    private float _maxCrystalSize;
    private Transform _closestTarget;
    [SerializeField] private LayerMask _whatIsEnemy;

    public void SetupCrystal(float crystalDuration, bool canExplode, bool canMove, float moveSpeed,
        float growSpeed, float maxCrystaSize, Transform closestTarget, Player player)
    {
        _crystalExistTimer = crystalDuration;
        _canExplode = canExplode;
        _canMove = canMove;
        _moveSpeed = moveSpeed;
        _growSpeed = growSpeed;
        _maxCrystalSize = maxCrystaSize;
        _closestTarget = closestTarget;
        _player = player;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.Instance.Blackhole.GetBlackHoleRadius();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, _whatIsEnemy);

        if (colliders.Length > 0)
            _closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }

    private void Update()
    {
        _crystalExistTimer -= Time.deltaTime;

        if (_crystalExistTimer < 0)
        {
            FinishCrystal();
        }

        if (_canMove)
        {
            if (_closestTarget == null)
                return;

            transform.position = Vector2.MoveTowards(transform.position, _closestTarget.position, _moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, _closestTarget.position) < 0.4f)
            {
                FinishCrystal();
                _canMove = false;
            }
        }

        if (_canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxCrystalSize, _maxCrystalSize), _growSpeed * Time.deltaTime);
    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                _player.Stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

            ItemDataEquipment equipedAmulet = PlayerInventory.Instance.GetEquipment(EquipmentType.Amulet);

            if (equipedAmulet != null)
                equipedAmulet.Effect(hit.transform);
        }
    }

    public void FinishCrystal()
    {
        _canGrow = true;

        if (_canExplode)
            _anim.SetTrigger("Explode");
        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}

