using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private SkillTreeSlotUI _unlockDodgeButton;
    [SerializeField] private int _evasionAmount;
    public bool DodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private SkillTreeSlotUI _unlockMirageDodge;
    public bool MirageDodgeUnlocked;

    protected override void Start()
    {
        base.Start();

        _unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
        _unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnlockMirageDodge);
    }

    protected override void CheckUnlock()
    {
        UnlockDodge();
        UnlockMirageDodge();
    }

    private void UnlockDodge()
    {
        if (_unlockMirageDodge&&!DodgeUnlocked)
        {
            player.Stats.Evasion.AddModifier(_evasionAmount);
            PlayerInventory.Instance.UpdateStatsUI();
            DodgeUnlocked = true;
        }
    }

    private void UnlockMirageDodge()
    {
        if (_unlockMirageDodge)
            MirageDodgeUnlocked = true;
    }

    public void CreateMiragenOnDodge()
    {
        if (MirageDodgeUnlocked)
            SkillManager.Instance.Clone.CreateClone(player.transform, new Vector3(1.2f*player.FacingDir,0));
    }
}
  