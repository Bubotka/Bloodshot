using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlotUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private UI _ui;

    [SerializeField] private string _statName;
    [SerializeField] private TextMeshProUGUI _statValueText;
    [SerializeField] private TextMeshProUGUI _statNameText;
    [SerializeField] private StatType _statType;

    [TextArea]
    [SerializeField] private string _statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + _statName;

        if (_statNameText != null)
            _statNameText.text = _statName;
    }

    private void Start()
    {
        UpdateStatValueUI();

        _ui = GetComponentInParent<UI>();
    }
      
    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.Instance.Player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            _statValueText.text = playerStats.GetStat(_statType).GetValue().ToString();

            if (_statType == StatType.Health)
                _statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if (_statType == StatType.Damage)
                _statValueText.text = (playerStats.Damage.GetValue() + playerStats.Strength.GetValue()).ToString(); 

            if (_statType == StatType.CritPower)
                _statValueText.text = (playerStats.CritPower.GetValue() + playerStats.Strength.GetValue()).ToString();

            if (_statType == StatType.CritChance)
                _statValueText.text = (playerStats.CritChance.GetValue() + playerStats.Agility.GetValue()).ToString();

            if (_statType == StatType.Evasion)
                _statValueText.text = (playerStats.Evasion.GetValue() + playerStats.Agility.GetValue()).ToString();

            if (_statType == StatType.MagicRes)
                _statValueText.text = (playerStats.MagicResistance.GetValue() + (playerStats.Intelligence.GetValue()*3).ToString());

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.StatToolTip.ShowStatToolTip(_statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ui.StatToolTip.HideStatToolTip();
    }
}
