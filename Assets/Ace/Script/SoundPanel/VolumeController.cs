using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _volumeValueText;
    [SerializeField]
    private Slider _slider;

    [SerializeField]
    private AudioSource _audioSource;

    const string MASTER_VOLUME = "MasterVolume";
    const string EFFECT_VOLUME = "EffectVolume";
    const string BACKGROUND_VOLUME = "BackGroundVolume";
    const string INPUT_VOLUME = "InputVolume";
    const string OUTPUT_VOLUME = "OutputVolume";
    private float _initVolume = 0.5f;

    private void Awake()
    {
        _slider = GetComponentInChildren<Slider>();

        GetValue(MASTER_VOLUME);
        //GetValue(EFFECT_VOLUME);
        //GetValue(BACKGROUND_VOLUME);
        //GetValue(INPUT_VOLUME);
        //GetValue(OUTPUT_VOLUME);
    }
    
    private void GetValue(string key)
    {
        if (PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetFloat(key, _initVolume);
        }
        _slider.value = PlayerPrefs.GetFloat(key);
    }

    public void MasterValueChanged(Slider slider)
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME, slider.value);
        _volumeValueText.text = (int)(PlayerPrefs.GetFloat(MASTER_VOLUME) * 100) + "%";
        // _audioSource.volume = slider.value;
    }
}
