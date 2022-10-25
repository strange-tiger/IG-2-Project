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
        if(collision.gameObject.tag == "Player")
        {
            if (photonView.IsMine)
            {
                photonView.RPC("AddSteppedCount", RpcTarget.All);
                Debug.Log("들어오라고!");
            }

            if (PhotonNetwork.IsMasterClient)
            {
                if (SteppedCount == 1)
                {
                   photonView.RPC("PlayPianoSound", RpcTarget.All);
                    //PlayPianoSound();
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (photonView.IsMine)
            {
                photonView.RPC("SubtractSteppedCount", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void PlayPianoSound()
    {
        _audioSource.PlayOneShot(_myAudioClip, _audioSource.volume
                    * PlayerPrefs.GetFloat("EffectVolume"));
    }

    [PunRPC]
    public void AddSteppedCount()
    {
        ++_steppedCount;
    }
    [PunRPC]
    public void SubtractSteppedCount()
    {
        --_steppedCount;
    }
}
