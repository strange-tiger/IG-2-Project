using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class PianoButton : MonoBehaviour
{
    [SerializeField]
    private AudioClip _myAudioClip = null;

    private AudioSource _audioSource;
    private int _steppedCount = 0;
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ++_steppedCount;
        if (_steppedCount == 1)
        {
            _audioSource.PlayOneShot(_myAudioClip, _audioSource.volume
                * PlayerPrefs.GetFloat("EffectVolume"));
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        --_steppedCount;
    }
}
