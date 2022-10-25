using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Photon.Pun;

public class PianoButton : MonoBehaviourPun
{
    [SerializeField]
    private AudioClip _myAudioClip = null;

    private AudioSource _audioSource;
    private int _steppedCount = 0;
    public int SteppedCount
    {
        get { return _steppedCount; }
        set { _steppedCount = value; }
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _myAudioClip;
        _audioSource.spatialBlend = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.IsMine)
        {
            ++_steppedCount;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            if (SteppedCount == 1)
            {
                photonView.RPC("PlayPianoSound", RpcTarget.All);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (photonView.IsMine)
        {
            --_steppedCount;
        }
    }

    [PunRPC]
    public void PlayPianoSound()
    {
        _audioSource.PlayOneShot(_myAudioClip, _audioSource.volume
                    * PlayerPrefs.GetFloat("EffectVolume"));
    }
}
