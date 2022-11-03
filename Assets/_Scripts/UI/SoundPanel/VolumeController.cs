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
    public AudioSource PlayerAudioSource { get; set; }

    private void OnEnable()
    {
        Debug.Log($"SoundManager.VOLUME_CONTROLLER.Length {SoundManager.VOLUME_CONTROLLER.Length}");
        for (int i = 0; i < SoundManager.VOLUME_CONTROLLER.Length; i++)
        {
            _textDict.Add(_slider[i], _text[i]);
            _slider[i].value = PlayerPrefs.GetFloat(SoundManager.VOLUME_CONTROLLER[i]);
        }
    }
    public void MasterValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat
            (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.MasterVolume], slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        SoundManager.Instance.Refresh();
    }
    public void EffectValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat
           (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.EffectVolume], slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        SoundManager.Instance.Refresh();
    }
    public void BackGroundValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat
           (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.BackGroundVolume], slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        SoundManager.Instance.Refresh();
    }
    public void InputValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat
           (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.InputVolume], slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        SoundManager.Instance.Refresh();
    }
    public void OutputValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat
            (SoundManager.VOLUME_CONTROLLER[(int)Defines.EVoiceUIType.OutputVolume], slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        //SoundManager.Instance.Refresh();
        // ¹Ì±¸Çö
    }
}