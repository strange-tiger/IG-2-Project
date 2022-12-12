using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Photon.Pun;
using Photon.Realtime;

public class PianoButton : MonoBehaviourPunCallbacks
{
    // 버튼 위에 플레이어가 올라가 있을 때 다른 플레이어가 같은 버튼을 밟으면 소리가 나선 안됨
    private int _steppedCount = 0;
    /// <summary>
    /// SteppedCount로 현재 올라온 플레이어 수를 세고 동기화 함
    /// </summary>
    public int SteppedCount
    {
        get { return _steppedCount; }
        set 
        { 
            photonView.RPC(nameof(SetSteppedCount), RpcTarget.AllBuffered, value); 
        }
    }

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
            SteppedCount++;

            if (SteppedCount == 1)
            {
                // 소리가 3D사운드기 때문에 소리는 MasterClient만 실행하도록 함
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