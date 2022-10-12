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
    
    void Awake()
    {
        _slider = GetComponentInChildren<Slider>();
        _slider.value = 0.5f;
    }

    void Update()
    {
        
    }

    public void ChangeValue(Slider slider)
    {
        _volumeValueText.text = (int)(slider.value * 100) + "%";
        _audioSource.volume = slider.value;
    }
}
