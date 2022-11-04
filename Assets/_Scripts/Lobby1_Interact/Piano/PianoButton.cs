using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Photon.Pun;
using Photon.Realtime;

public class PianoButton : MonoBehaviourPunCallbacks
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
        if (!PlayerPrefs.HasKey("EffectVolume"))
        {
            PlayerPrefs.SetFloat("EffectVolume", 50f);
        }

        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _myAudioClip;
        _audioSource.spatialBlend = 1;
    }
    
    [PunRPC]
    private void PlayerJoined(int steppedCount)
    {
        SteppedCount = steppedCount;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC("PlayerJoined", newPlayer, SteppedCount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBody")
        {
            photonView.RPC("AddSteppedCount", RpcTarget.All);

            if (SteppedCount == 1)
            {
                photonView.RPC("PlayPianoSound", RpcTarget.All);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBody")
        {
            photonView.RPC("SubtractSteppedCount", RpcTarget.All);
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
        Debug.Log("[Sound] " + _steppedCount);
    }
    [PunRPC]
    public void SubtractSteppedCount()
    {
        --_steppedCount;
        Debug.Log("[Sound] " + _steppedCount);
    }
}