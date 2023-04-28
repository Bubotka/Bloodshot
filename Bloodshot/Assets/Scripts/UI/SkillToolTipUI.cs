using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillToolTipUI : ToolTipUI
{
    [SerializeField] private TextMeshProUGUI _skillText;
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillCost;

    public void ShowToolTip(string skillDescription,string skillName,int price)
    {
        _skillName.text = skillName;
        _skillText.text = skillDescription;
        _skillCost.text = "Cost: " + price;

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
