using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private GameObject _skillTreeUI;
    [SerializeField] private GameObject _optionsUI;
    [SerializeField] private GameObject _inGameUI;

    public ItemToolTipUI ItemToolTip;
    public StatTooltipUI StatToolTip;
    public SkillToolTipUI SkillToolTip;

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
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
            menu.SetActive(true);
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
        for (int i  = 0; i  < transform.childCount; i ++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(_inGameUI);  
    }
}
