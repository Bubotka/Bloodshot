using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private bool _canCreateCloneOnCounterAttack;
    [Header("Clone can duplicate")]
    [SerializeField] private float _chanceToDuplicate;
    [SerializeField] private bool _canDuplicateClone;
    [Header("Crystal instead of clone")]
    public bool CrystallInsteadOfClone;

    public void CreateClone(Transform clonePosition,Vector3 offset)
    {
        if (CrystallInsteadOfClone)
        {
            SkillManager.Instance.Crystal.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(_clonePrefab,player.transform.position,Quaternion.identity);

        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, _cloneDuration,
            _canAttack, offset, FindClosestEnemy(newClone.transform), _canDuplicateClone, _chanceToDuplicate);
    }

    public void CreateCloneOnDashStart()
    {
        if (_createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (_createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform)
    {
        if (_canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWhithDelay(enemyTransform, new Vector3(1 * player.FacingDir, 0)));
    }

    private IEnumerator CreateCloneWhithDelay(Transform transform,Vector3 offset)
    {
        yield return new WaitForSeconds(0.4f);
            CreateClone(transform, offset);
            
    }
}
