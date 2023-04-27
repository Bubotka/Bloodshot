using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private SkillTreeSlotUI _dashUnlockButton;

    public bool DashUnlocked { get; private set; }

    [Header("Clone on dash")]
    [SerializeField] private SkillTreeSlotUI _cloneDashUnclockButton;

    public bool CloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private SkillTreeSlotUI _cloneOnArrivalUnlockButton;

    public bool CloneOnArrivalUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();
    }

    protected override void Start()
    {
        base.Start();

        _dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        _cloneDashUnclockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        _cloneOnArrivalUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnArrival);
    }

    private void UnlockDash()
    {
        if (_dashUnlockButton.Unlocked)
        {
            DashUnlocked = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if (_cloneDashUnclockButton.Unlocked) 
            CloneOnDashUnlocked = true;
    }

    private void UnlockCloneOnArrival()
    {
        if (_cloneOnArrivalUnlockButton.Unlocked)
            CloneOnArrivalUnlocked = true;
    }
     
    public void CloneOnDash()
    {
        if (CloneOnDashUnlocked)
            SkillManager.Instance.Clone.CreateClone(player.transform, Vector3.zero);
    }

    public void CloneOnArrival()
    {
        if (CloneOnArrivalUnlocked)
            SkillManager.Instance.Clone.CreateClone(player.transform, Vector3.zero);
    }
}
