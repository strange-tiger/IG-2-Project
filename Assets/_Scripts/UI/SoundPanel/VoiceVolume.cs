using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VoiceVolume : MonoBehaviourPun
{
    private AudioSource _voicePlayer;
    private float _localVolume = 0;
    private float _remoteVolume = 0;

    private void Awake()
    {
        _voicePlayer = GetComponent<AudioSource>();
        SoundManager.Instance.OnChangedInputVolume.RemoveListener(UpdateInputVolume);
        SoundManager.Instance.OnChangedOutputVolume.RemoveListener(UpdateOutputVolume);
        SoundManager.Instance.OnChangedInputVolume.AddListener(UpdateInputVolume);
        SoundManager.Instance.OnChangedOutputVolume.AddListener(UpdateOutputVolume);
    }

    public void UpdateInputVolume(float inputVolume)
    {
        if(photonView.IsMine)
        {
            photonView.RPC(nameof(ChangedVolume), RpcTarget.All, inputVolume);
        }
    }

    [PunRPC]
    private void ChangedVolume(float inputVolume)
    {
        _localVolume = inputVolume;
        if(photonView.IsMine)
        {
            _voicePlayer.volume = inputVolume;
        }
        else
        {
            _voicePlayer.volume = inputVolume * _remoteVolume;
        }
    }

    public void UpdateOutputVolume(float outputVolume)
    {
        _remoteVolume = outputVolume;
        if(photonView.IsMine)
        {
            return;
        }
        _voicePlayer.volume = _localVolume * outputVolume;
    }

    private void OnDestroy()
    {
        SoundManager.Instance.OnChangedInputVolume?.RemoveListener(UpdateInputVolume);
    }
}
