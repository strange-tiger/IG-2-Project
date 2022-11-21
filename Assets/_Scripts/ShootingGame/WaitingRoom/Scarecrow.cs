using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Scarecrow : MonoBehaviourPun
{
    private AudioSource _audioSource;
    [SerializeField]
    private ParticleSystem _hitEffect;

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
        //hitPoint 위치에 효과를 재생시키면 된다
        _hitEffect.transform.position = hitPoint;
        _hitEffect.Play();
        _audioSource.Play();
    }
}
