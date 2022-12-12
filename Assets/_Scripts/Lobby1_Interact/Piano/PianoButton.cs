using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using Photon.Pun;
using Photon.Realtime;

public class PianoButton : MonoBehaviourPunCallbacks
{
    // ��ư ���� �÷��̾ �ö� ���� �� �ٸ� �÷��̾ ���� ��ư�� ������ �Ҹ��� ���� �ȵ�
    private int _steppedCount = 0;
    /// <summary>
    /// SteppedCount�� ���� �ö�� �÷��̾� ���� ���� ����ȭ ��
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
                // �Ҹ��� 3D����� ������ �Ҹ��� MasterClient�� �����ϵ��� ��
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