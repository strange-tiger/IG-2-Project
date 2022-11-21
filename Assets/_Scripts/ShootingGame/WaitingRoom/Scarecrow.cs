using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Scarecrow : MonoBehaviourPun
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Hit(Vector3 hitPoint)
    {
        photonView.RPC(nameof(PlayEffect), RpcTarget.All, hitPoint);
    }

    [PunRPC]
    private void PlayEffect(Vector3 hitPoint)
    {
        //hitPoint ��ġ�� ȿ���� �����Ű�� �ȴ�

        _audioSource.Play();
    }
}
