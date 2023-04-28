using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class HealthBarUI : MonoBehaviour
{
    private Entity _entity;
    private RectTransform _myTransform;
    private Slider _slider;
    private CharacterStats _myStats;

    private void Start()
    {
        _myTransform = GetComponent<RectTransform>();
        _entity = GetComponentInParent<Entity>();
        _slider = GetComponentInChildren<Slider>();
        _myStats = GetComponentInParent<CharacterStats>();

        _entity.OnFlipped += FlipUI;
        _myStats.HealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void OnDisable()
    {
        _entity.OnFlipped -= FlipUI;
        _myStats.HealthChanged -= UpdateHealthUI;
    }

    private void FlipUI()=> _myTransform.Rotate(0, 180, 0);

    private void UpdateHealthUI()
    {
        _slider.maxValue = _myStats.GetMaxHealthValue();
        _slider.value = _myStats.CurrentHealth;
    }


}
