using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class Tu1_PianoButton : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1;
        _audioSource.volume = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBody"))
        {
            PlayPianoSound();
        }
    }
    private void PlayPianoSound()
    {
        _audioSource.Play();
    }
}