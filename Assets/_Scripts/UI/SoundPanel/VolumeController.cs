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

    private void OnDisable()
    {
        for (int i = 0; i < SoundManager.VOLUME_CONTROLLER.Length; i++)
        {
            Debug.Log(i);

            PlayerPrefs.SetFloat(SoundManager.VOLUME_CONTROLLER[i], _slider[i].value);
            SoundManager.Instance.Refresh(i);
        }
    }
    public void VolumeValueChanged(Slider slider)
    {
        _textDict[slider].text = ((int)(slider.value * 100)).ToString() + "%";
    }
}