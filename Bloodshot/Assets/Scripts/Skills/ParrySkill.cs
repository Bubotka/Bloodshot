using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] private SkillTreeSlotUI _parryUnlockButton;

    public bool ParryUnlocked { get; private set; }

    [Header("Parry restore")]
    [SerializeField] private SkillTreeSlotUI _restoreUnlockButton;
    [Range(0f, 1f)]
    [SerializeField] private float _restoreHealthPercentage; 
    public bool RestoreUnlocked { get; private set; }

    [Header("Parry with mirage")]
    [SerializeField] private SkillTreeSlotUI _parryWithMirageUnlockButton;
    public bool ParryWithMirageUnlocked { get; private set; }

    public override void UseSkill()
    {
        base.UseSkill();

        if (RestoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.Stats.GetMaxHealthValue() * _restoreHealthPercentage);
            player.Stats.IncreaseHealthBy(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        _parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        _restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        _parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
    }

    protected override void CheckUnlock()
    {
        UnlockParry();
        UnlockParryRestore();
        UnlockParryWithMirage();
    }

    private void UnlockParry()
    {
        if (_parryUnlockButton.Unlocked)
            ParryUnlocked = true;
    }

    private void UnlockParryRestore()
    {
        if (_restoreUnlockButton)
            RestoreUnlocked = true;
    }

    private void UnlockParryWithMirage()
    {
        if (_parryWithMirageUnlockButton)
            ParryWithMirageUnlocked = true;
    }

    public void MakeMirageOnParry(Transform respawnTransform)
    {
        if (ParryWithMirageUnlocked)
            SkillManager.Instance.Clone.CreateCloneWithDelay(respawnTransform);
    }
}
