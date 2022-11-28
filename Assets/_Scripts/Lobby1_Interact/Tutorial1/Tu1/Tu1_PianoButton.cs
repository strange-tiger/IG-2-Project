using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Photon.Pun;
using Photon.Realtime;

public class Tu1_PianoButton : MonoBehaviourPunCallbacks
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