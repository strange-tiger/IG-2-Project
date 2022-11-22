using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Photon.Pun;
using Photon.Realtime;

public class PianoButton : MonoBehaviourPunCallbacks
{
    private AudioSource _audioSource;

    private int _steppedCount = 0;
    public int SteppedCount
    {
        get { return _steppedCount; }
        set 
        { 
            photonView.RPC(nameof(SetSteppedCount), RpcTarget.All); 
        }
    }
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.spatialBlend = 1;
        _audioSource.volume = 1;
    }
    
    [PunRPC]
    private void PlayerJoined(int steppedCount)
    {
        _steppedCount = steppedCount;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        photonView.RPC(nameof(PlayerJoined), newPlayer, SteppedCount);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBody"))
        {
            SteppedCount++;

            if (SteppedCount == 1)
            {
                photonView.RPC(nameof(PlayPianoSound),RpcTarget.MasterClient);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBody"))
        {
            SteppedCount--;
        }
    }

    [PunRPC]
    public void PlayPianoSound()
    {
        _audioSource.Play();
    }

    [PunRPC]
    public void SetSteppedCount(int value)
    {
        _steppedCount = value;
    }
}