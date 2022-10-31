using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UINum = Defines.EVoiceUIType;

public class BGM : MonoBehaviour
{
    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 
            PlayerPrefs.GetFloat(VolumeController.VOLUME_CONTROLLER[(int)UINum.BackGroundVolume]);
    }

    void Update()
    {
        _audioSource.volume = 
            PlayerPrefs.GetFloat(VolumeController.VOLUME_CONTROLLER[(int)UINum.BackGroundVolume]);
    }
}
