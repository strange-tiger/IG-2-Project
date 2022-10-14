using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using UINum = Defines.EVoiceUIType;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] _text;
    [SerializeField]
    private Slider[] _slider;

    private Dictionary<string, Slider> _sliderDict = new Dictionary<string, Slider>();
    private Dictionary<Slider, TextMeshProUGUI> _textDict = new Dictionary<Slider, TextMeshProUGUI>();

    [SerializeField]
    private AudioSource _audioSource;

    private readonly string[] VOLUME_CONTROLLER = 
        { "MasterVolume", "EffectVolume", "BackGroundVolume", "InputVolume", "OutputVolume" };


    private void Start()
    {
        for (int i = 0; i < (int)UINum.MaxCount; i++)
        {
            _textDict.Add(_slider[i], _text[i]);
            _sliderDict.Add(VOLUME_CONTROLLER[i], _slider[i]);
            GetValue(VOLUME_CONTROLLER[i]);
        }
    }

    private float _initVolume = 0.5f;
    private void GetValue(string key)
    {
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetFloat(key, _initVolume);
        }
        _sliderDict[key].value = PlayerPrefs.GetFloat(key);
    }

    public void MasterValueChanged(Slider slider)
    {
        string MasterVolume = VOLUME_CONTROLLER[(int)UINum.MasterVolume];
        PlayerPrefs.SetFloat(MasterVolume, slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        //_audioSource.volume = slider.value;
    }
    public void EffectValueChanged(Slider slider)
    {
        string EffectVolume = VOLUME_CONTROLLER[(int)UINum.EffectVolume];
        PlayerPrefs.SetFloat(EffectVolume, slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        //_audioSource.volume = slider.value;
    }
    public void BackGroundValueChanged(Slider slider)
    {
        string BackGroundVolume = VOLUME_CONTROLLER[(int)UINum.BackGroundVolume];
        PlayerPrefs.SetFloat(BackGroundVolume, slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        _audioSource.volume = slider.value;
    }
    public void InputValueChanged(Slider slider)
    {
        string InputVolume = VOLUME_CONTROLLER[(int)UINum.InputVolume];
        PlayerPrefs.SetFloat(InputVolume, slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        //_audioSource.volume = slider.value;
    }
    public void OutputValueChanged(Slider slider)
    {
        string OutputVolume = VOLUME_CONTROLLER[(int)UINum.OutputVolume];
        PlayerPrefs.SetFloat(OutputVolume, slider.value);
        _textDict[slider].text = (int)(slider.value * 100) + "%";
        //_audioSource.volume = slider.value;
    }
}