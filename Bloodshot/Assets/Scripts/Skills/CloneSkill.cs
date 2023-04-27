using System.Collections;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]

    [SerializeField] private float _cloneDuration;
    [SerializeField] private GameObject _clonePrefab;
    [Space]
    [SerializeField] private bool _canAttack;

    [SerializeField] private bool _createCloneOnDashStart;
    [SerializeField] private bool _createCloneOnDashOver;
    [Header("Clone can duplicate")]
    [SerializeField] private float _chanceToDuplicate;
    [SerializeField] private bool _canDuplicateClone;
    [Header("Crystal instead of clone")]
    public bool CrystallInsteadOfClone;

    public void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (CrystallInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(_clonePrefab, player.transform.position, Quaternion.identity);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, _cloneDuration,
            _canAttack, offset, FindClosestEnemy(newClone.transform), _canDuplicateClone, _chanceToDuplicate, player);
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
