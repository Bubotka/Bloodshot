using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatTooltipUI : ToolTipUI
{
    [SerializeField] private TextMeshProUGUI _description;

    public void ShowStatToolTip(string text)
    {
        _description.text = text;
        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        _description.text = "";

        gameObject.SetActive(false);
    }
}
