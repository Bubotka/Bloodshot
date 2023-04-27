using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillToolTipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _skillText;
    [SerializeField] private TextMeshProUGUI _skillName;

    public void ShowToolTip(string skillDescription,string skillName)
    {
        _skillName.text = skillName;
        _skillText.text = skillDescription;
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
