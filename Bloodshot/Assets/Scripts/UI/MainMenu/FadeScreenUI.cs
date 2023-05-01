using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScreenUI : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void FadeOut() => _anim.SetTrigger("FadeOut");

    public void FadeIn() => _anim.SetTrigger("FadeIn");
} 
