using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private FadeScreenUI _fadeScreen;
    [SerializeField] private GameObject _endText;
    [Space]
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private GameObject _skillTreeUI;
    [SerializeField] private GameObject _optionsUI;
    [SerializeField] private GameObject _inGameUI;

    public ItemToolTipUI ItemToolTip;
    public StatTooltipUI StatToolTip;
    public SkillToolTipUI SkillToolTip;

    [SerializeField] private VolumeSliderUI[] _volumeSettings;

    private void Awake()
    {
        SwitchTo(_skillTreeUI);
    }

    private void Start()
    {
        SwitchTo(_inGameUI);

        ItemToolTip.gameObject.SetActive(false);
        StatToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(_characterUI); 

        if (Input.GetKeyDown(KeyCode.K))
            SwitchWithKeyTo(_skillTreeUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(_optionsUI);
    }

    public void SwitchTo(GameObject menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<FadeScreenUI>() != null;

            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            AudioManager.Instance.PlaySFX(8, null);
            menu.SetActive(true);
        }

        if (GameManager._instance != null)
        {
            if (menu == _inGameUI)
                GameManager._instance.PauseGame(false);
            else
                GameManager._instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUi();
            return;
        }
        SwitchTo(menu);
    }

    private void CheckForInGameUi()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf&&transform.GetChild(i).GetComponent<FadeScreenUI>()==null)
                return;
        }

        SwitchTo(_inGameUI); 
    }

    public void SwitchOnEndScreen()
    {
        _fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
        StartCoroutine(RestartGameCoroutine());
    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        _endText.SetActive(true);
    }

    IEnumerator RestartGameCoroutine()
    {
        yield return new WaitForSeconds(2.5f);
        RestartGame();
    }

    public void RestartGame() => GameManager._instance.RestartScene();

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string,float> pair in data.VolumeSettings)
        {
            foreach (VolumeSliderUI item in _volumeSettings)
            {
                if (item.Parametr == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }
     
    public void SaveData(ref GameData data)
    {
        data.VolumeSettings.Clear();
         
        foreach (VolumeSliderUI item in _volumeSettings)
        {
            data.VolumeSettings.Add(item.Parametr, item.Slider.value);
        }
    }
}
 