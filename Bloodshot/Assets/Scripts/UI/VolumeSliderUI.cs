using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderUI : MonoBehaviour
{
    public Slider Slider;
    public string Parametr;

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private float _multiplier;

    public void SliderValue() => _audioMixer.SetFloat(Parametr, Mathf.Log10(Slider.value) * _multiplier);

    public void LoadSlider(float value)
    {
        if (value >= 0.001f) 
            Slider.value = value;
    }
}
 