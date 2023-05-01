using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISaveManager
{
    private UI _ui;
    private Image _skillImage;

    [SerializeField] private int _skillPrice;
    [SerializeField] private string _skillName;
    [TextArea]
    [SerializeField] private string _skillDescription;
    [SerializeField] private Color _lockedSkillColor;

    [SerializeField] private SkillTreeSlotUI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlotUI[] shouldBeLocked;

    public bool Unlocked;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI" + _skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        _skillImage = GetComponent<Image>();
        _ui = GetComponentInParent<UI>();
        
        _skillImage.color = _lockedSkillColor;

        if (Unlocked)
            _skillImage.color = Color.white;
    }

    public void UnlockSkillSlot()
    {
        if (PlayerManager.Instance.HaveEnoughMoney(_skillPrice) == false)
            return;

        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].Unlocked == false)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].Unlocked == true)
            {
                Debug.Log("Cannot unlock skill");
                return;
            }
        }

        Unlocked = true;
        _skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.SkillToolTip.ShowToolTip(_skillDescription, _skillName, _skillPrice);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.SkillToolTip.HideToolTip();
    }

    public void LoadData(GameData data)
    {
        if(data.SkillTree.TryGetValue(_skillName,out bool value))
        {
            Unlocked = value;
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data.SkillTree.TryGetValue(_skillName, out bool value))
        {
            data.SkillTree.Remove(_skillName);
            data.SkillTree.Add(_skillName, Unlocked);
        }
        else
        {
            data.SkillTree.Add(_skillName, Unlocked);
        }
    }
}
