using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InputVoiceVolume : MonoBehaviourPun
{
    private AudioSource _voicePlayer;
    
    private PhotonView _photonview;

    private void Awake()
    {
        _voicePlayer = GetComponent<AudioSource>();
        _photonview = GetComponentInParent<PhotonView>();
        SoundManager.Instance.OnChangedInputVolume.RemoveListener(UpdateVolume);
        SoundManager.Instance.OnChangedInputVolume.AddListener(UpdateVolume);
    }

    [PunRPC]
    public void UpdateVolume(float voiceVolume)
    {
        if(_photonview.IsMine)
        {
            _photonview.RPC(nameof(UpdateVolume), RpcTarget.Others, voiceVolume);
        }
        _voicePlayer.volume = voiceVolume;
    }

    private void OnDestroy()
    {
        SoundManager.Instance.OnChangedInputVolume?.RemoveListener(UpdateVolume);
    }
}
