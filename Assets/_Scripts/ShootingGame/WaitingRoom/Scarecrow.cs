using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Scarecrow : MonoBehaviourPun
{
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _hitEffect;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Hit(Vector3 hitPoint)
    {
        photonView.RPC(nameof(PlayEffect), RpcTarget.All, hitPoint);
    }

    //배열 사용해서 오브젝트 풀링
    [PunRPC]
    private void PlayEffect(Vector3 hitPoint)
    {
        //hitPoint 위치에 효과를 재생시키면 된다
        _hitEffect.transform.position = hitPoint;
        
        _audioSource.Play();
    }
}
