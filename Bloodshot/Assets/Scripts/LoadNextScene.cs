using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    [SerializeField] private string _sceneName = "TrainingPlace";


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
            StartCoroutine(LoadScreenWithFadeEffect(1));
    }

    IEnumerator LoadScreenWithFadeEffect(float delay)
    {


        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(_sceneName);
    }
}
