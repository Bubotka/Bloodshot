using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Slider _slider;

    [SerializeField] private Image _dashImage;
    [SerializeField] private Image _parryImage;
    [SerializeField] private Image _crystalImage;
    [SerializeField] private Image _swordImage;
    [SerializeField] private Image _blackHoleImage;
    [SerializeField] private Image _flaskImage;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI _currentSouls;
    [SerializeField] private float _soulsAmount;
    [SerializeField] private float _increaseRate=100;

    private SkillManager _skills;
    private void Start()
    {
        if (_playerStats != null) 
            _playerStats.HealthChanged += UpdateHealthUI;

        _skills = SkillManager.Instance;
    }

    private void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && _skills.Dash.DashUnlocked)
            SetCooldownOf(_dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && _skills.Parry.ParryUnlocked)
            SetCooldownOf(_parryImage);

        if (Input.GetKeyDown(KeyCode.F) && _skills.Crystal.CrystalUnlocked)
            SetCooldownOf(_crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && _skills.Sword.SwordUnlocked)
            SetCooldownOf(_swordImage);

        if (Input.GetKeyDown(KeyCode.R) && _skills.Blackhole.BlackHoleUnlocked)
            SetCooldownOf(_blackHoleImage);

        if (Input.GetKeyDown(KeyCode.Alpha1) && PlayerInventory.Instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(_flaskImage);

        CheckCooldownOf(_dashImage, _skills.Dash.Cooldown);
        CheckCooldownOf(_parryImage, _skills.Parry.Cooldown);
        CheckCooldownOf(_crystalImage, _skills.Crystal.Cooldown);
        CheckCooldownOf(_swordImage, _skills.Sword.Cooldown);
        CheckCooldownOf(_blackHoleImage, _skills.Blackhole.Cooldown);
        CheckCooldownOf(_flaskImage, PlayerInventory.Instance.FlaskCooldown);
    }

    private void UpdateSoulsUI()
    {
        if (_soulsAmount < PlayerManager.Instance.GetCurrency())
            _soulsAmount += Time.deltaTime * _increaseRate;
        else
            _soulsAmount = PlayerManager.Instance.GetCurrency();

        _currentSouls.text = ((int)_soulsAmount).ToString();
    }

    private void UpdateHealthUI()
    {
        _slider.maxValue = _playerStats.GetMaxHealthValue();
        _slider.value = _playerStats.CurrentHealth;
    }

    private void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
            image.fillAmount = 1;
    } 

    private void CheckCooldownOf(Image image,float cooldown)
    {
        if (image.fillAmount > 0)
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
    }
}
