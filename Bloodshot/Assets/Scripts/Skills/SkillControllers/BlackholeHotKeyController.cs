using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackholeHotKeyController : MonoBehaviour
{
    private SpriteRenderer _sr;
    private KeyCode _myHotKey;
    private TextMeshProUGUI _myText;

    private Transform _myEnemy;
    private BlackholeSkillController _blackHole;

    public void SetupHotKey(KeyCode myNewHotKey, Transform myEnemy, BlackholeSkillController myBlackHole    )
    {
        _sr = GetComponent<SpriteRenderer>();
        _myText = GetComponentInChildren<TextMeshProUGUI>();

        _myEnemy = myEnemy;
        _blackHole = myBlackHole;

        _myHotKey = myNewHotKey;
        _myText.text = myNewHotKey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_myHotKey))
        {
            _blackHole.AddEnemyToList(_myEnemy);

            _myText.color = Color.clear;
            _sr.color = Color.clear;
        }
    }
}
