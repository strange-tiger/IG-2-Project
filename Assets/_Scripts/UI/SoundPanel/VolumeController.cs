using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _text;
    [SerializeField]
    private Slider[] _slider;

    private Dictionary<Slider, TextMeshProUGUI> _textDict = new Dictionary<Slider, TextMeshProUGUI>();

    private void Awake()
    {
        for (int i = 0; i < SoundManager.VOLUME_CONTROLLER.Length; i++)
        {
            _textDict.Add(_slider[i], _text[i]);
        }
    }
    private void OnEnable()
    {
        for (int i = 0; i < SoundManager.VOLUME_CONTROLLER.Length; i++)
        {
            _slider[i].value = PlayerPrefs.GetFloat(SoundManager.VOLUME_CONTROLLER[i]);
        }

    }
    public void MasterValueChanged(Slider slider)
    {
        _textDict[slider].text = ((int)(slider.value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat
            (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.MasterVolume], slider.value);
        SoundManager.Instance.Refresh((int)Defines.EVoiceUIType.MasterVolume);
    }
    public void EffectValueChanged(Slider slider)
    {
        _textDict[slider].text = ((int)(slider.value * 100)).ToString()+ "%";
        PlayerPrefs.SetFloat
           (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.EffectVolume], slider.value);
        SoundManager.Instance.Refresh((int)Defines.EVoiceUIType.EffectVolume);
    }
    public void BackgroundValueChanged(Slider slider)
    {
        _textDict[slider].text = ((int)(slider.value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat
           (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.BackGroundVolume], slider.value);
        SoundManager.Instance.Refresh((int)Defines.EVoiceUIType.BackGroundVolume);
    }
    public void InputValueChanged(Slider slider)
    {
        _textDict[slider].text = ((int)(slider.value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat
           (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.InputVolume], slider.value);
        SoundManager.Instance.Refresh((int)Defines.EVoiceUIType.InputVolume);
    }
    public void OutputValueChanged(Slider slider)
    {
        _textDict[slider].text = ((int)(slider.value * 100)).ToString() + "%";
        PlayerPrefs.SetFloat
            (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.OutputVolume], slider.value);
        SoundManager.Instance.Refresh((int)Defines.EVoiceUIType.OutputVolume);
    }
}