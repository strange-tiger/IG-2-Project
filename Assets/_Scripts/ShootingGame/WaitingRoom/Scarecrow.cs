using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

public class Scarecrow : MonoBehaviourPun
{
    [Header("미리 생성해놓은 파티클")]
    [SerializeField]
    private List<GameObject> _hitEffects = new List<GameObject>();

    private int _currentIndex = 0;
    private float _resetCoolTime = 0.5f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Hit(Vector3 hitPoint)
    {
        Debug.Log("피격당함");
        photonView.RPC(nameof(PlayEffect), RpcTarget.All, hitPoint);
    }

    [PunRPC]
    private void PlayEffect(Vector3 hitPoint)
    {
        Debug.Log("이펙트 재생");
        //hitPoint 위치에 효과를 재생시키면 된다
        _hitEffects[_currentIndex].gameObject.SetActive(false);
        _hitEffects[_currentIndex].gameObject.transform.position = hitPoint;
        _hitEffects[_currentIndex].gameObject.SetActive(true);
        _audioSource.Play();

        _currentIndex = (_currentIndex + 1) % _hitEffects.Count;
    }
}
