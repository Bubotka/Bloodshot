using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer _sr;
    private float _colorLooseRate;

    public void SetupAfterImage(float loosingSpeed, Sprite spriteImage)
    {
        _sr = GetComponent<SpriteRenderer>();

        _sr.sprite = spriteImage;
        _colorLooseRate = loosingSpeed;
       
    }

    private void Update()
    {
        float alpha = _sr.color.a - _colorLooseRate * Time.deltaTime;
        _sr.color = new Color(_sr.color.r, _sr.color.g, _sr.color.b, alpha);

        if (_sr.color.a <= 0)
            Destroy(gameObject);
    }
}
