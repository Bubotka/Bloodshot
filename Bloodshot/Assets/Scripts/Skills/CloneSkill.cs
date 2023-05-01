using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private float _attackMultiplier;
    [SerializeField] private float _cloneDuration;
    [SerializeField] private GameObject _clonePrefab;
    [Space]

    [Header("Clone attack")]
    [SerializeField] private SkillTreeSlotUI _cloneAttackUnlockButton; 
    [SerializeField] private float _cloneAttackMultiplier;
    [SerializeField] private bool _canAttack;
     
    [Header("Aggresive clone")]
    [SerializeField] private SkillTreeSlotUI _aggressiveCloneUnlockButton;
    [SerializeField] private float _aggresiveCLoneAttackMultiplier;

    public bool CanApplyOnHitEffect { get; private set; }

    [Header("Multiple clone")]
    [SerializeField] private SkillTreeSlotUI _multipleUnlockButton;
    [SerializeField] private float _multiCloneAttackMultiplier;
    [SerializeField] private float _chanceToDuplicate;
    [SerializeField] private bool _canDuplicateClone;
    [Header("Crystal instead of clone")]
    [SerializeField] private SkillTreeSlotUI _crystalInsteadUnlockButton;
    public bool CrystallInsteadOfClone;

    protected override void Start()
    {
        base.Start();

        _cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        _aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
        _multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        _crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
    }

    #region Unlock region

    private void UnlockCloneAttack()
    {
        if (_cloneAttackUnlockButton.Unlocked)
        {
            _canAttack = true;
            _attackMultiplier = _cloneAttackMultiplier;
        }
    }

    private void UnlockAggresiveClone()
    {
        if (_aggressiveCloneUnlockButton.Unlocked)
        {
            CanApplyOnHitEffect = true;
            _attackMultiplier = _aggresiveCLoneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (_multipleUnlockButton.Unlocked)
        {
            _canDuplicateClone = true;
            _attackMultiplier = _multiCloneAttackMultiplier;
        }
    } 

    private void UnlockCrystalInstead()
    {
        if (_crystalInsteadUnlockButton.Unlocked)
        {
            CrystallInsteadOfClone = true;
        }
    }
    #endregion

    protected override void CheckUnlock()
    {
        UnlockAggresiveClone();
        UnlockCloneAttack();
        UnlockCrystalInstead();
        UnlockMultiClone();
    }

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (CrystallInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(_clonePrefab, player.transform.position, Quaternion.identity);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, _cloneDuration,
            _canAttack, offset, FindClosestEnemy(newClone.transform), _canDuplicateClone, _chanceToDuplicate, player,_attackMultiplier);
    }

    public void CreateCloneWithDelay(Transform enemyTransform)
    {
        StartCoroutine(CloneDelayCoroutine(enemyTransform, new Vector3(1 * player.FacingDir, 0)));
    }

    private IEnumerator CloneDelayCoroutine(Transform transform, Vector3 offset)
    {
        yield return new WaitForSeconds(0.4f);
        CreateClone(transform, offset);
    }
}
