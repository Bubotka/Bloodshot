using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string _sceneName = "MainScene";
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private FadeScreenUI _fadeScreen;

    private void Start()
    {
        if (SaveManager.Instance.HasSavedData() == false)
            _continueButton.SetActive(false);
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1f));
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        StartCoroutine(LoadScreenWithFadeEffect(1f));
    }

    public void ExitGame()
    {
        Application.Quit(); 

    }

    IEnumerator LoadScreenWithFadeEffect(float delay)
    {
        _fadeScreen.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(_sceneName);
    }
}
