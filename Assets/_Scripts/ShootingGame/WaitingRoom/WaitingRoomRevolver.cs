using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomRevolver : MonoBehaviour
{
    private bool _isGrabbed = false;

    private ParticleSystem[] _shootEffects = new ParticleSystem[2];
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _shootEffects = GetComponentsInChildren<ParticleSystem>();
    }

    void Update()
    {
        if(_isGrabbed == false)
        {
            return;
        }


    }

    public void OnGrabBegin()
    {
        _isGrabbed = true;
    }

    public void OnGrabEnd()
    {
        _isGrabbed = false;
    }
    
}
