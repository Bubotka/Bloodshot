using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintUI : MonoBehaviour
{
    [SerializeField] private GameObject _hint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HintOpen();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HintClose();
        }
    }

    public void HintOpen()
    {
        _hint.SetActive(true);
    }

    public void HintClose()
    {
        _hint.SetActive(false);
    }
}
