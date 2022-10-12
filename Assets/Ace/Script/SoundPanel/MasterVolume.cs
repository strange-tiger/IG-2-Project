using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MasterVolume : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _volumeValueText;
    private Slider _slider;

    [SerializeField]
    private AudioSource _audioSource;

    const string MASTER_VOLUME = "MasterVolume";
    const string EFFECT_VOLUME = "EffectVolume";
    const string BACKGROUND_VOLUME = "BackGroundVolume";
    const string INPUT_VOLUME = "InputVolume";
    const string OUTPUT_VOLUME = "OutputVolume";
    private float _initVolume = 0.5f;

    void Awake()
    {
        _slider = GetComponentInChildren<Slider>();

        SetNewValue(MASTER_VOLUME, _initVolume);
        SetNewValue(EFFECT_VOLUME, _initVolume);
        SetNewValue(BACKGROUND_VOLUME, _initVolume);
        SetNewValue(INPUT_VOLUME, _initVolume);
        SetNewValue(OUTPUT_VOLUME, _initVolume);
    }
    
    void SetNewValue(string key, float value)
    {
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }

    public void ChangeValue(Slider slider)
    {
        _volumeValueText.text = (int)(slider.value * 100) + "%";
        //_audioSource.volume = slider.value;
    }
}
