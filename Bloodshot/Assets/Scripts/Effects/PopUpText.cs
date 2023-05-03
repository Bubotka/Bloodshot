using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private TextMeshPro _myText;

    [SerializeField] private float _speed;
    [SerializeField] private float _desapearanceSpeed;
    [SerializeField] private float _colorDesapearanceSpeed;

    [SerializeField] private float _lifeTime;

    private float _textTimer;

    private void Start()
    {
        _myText = GetComponent<TextMeshPro>();
        _textTimer = _lifeTime;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            new Vector2(transform.position.x, transform.position.y + 1), _speed * Time.deltaTime);

        _textTimer -= Time.deltaTime;

        if (_textTimer < 0)
        {
            float alpha = _myText.color.a - _colorDesapearanceSpeed * Time.deltaTime;
            _myText.color = new Color(_myText.color.r, _myText.color.g, _myText.color.b, alpha);

            if (_myText.color.a < 50)
                _speed = _desapearanceSpeed;

            if (_myText.color.a <= 0)
                Destroy(gameObject);
        } 
    }
}
