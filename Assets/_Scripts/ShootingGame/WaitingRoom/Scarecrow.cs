using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

public class Scarecrow : MonoBehaviourPun
{
    [SerializeField]
    private GameObject _hitEffect;
    [SerializeField]
    private int _count = 6;
    [SerializeField]
    private GameObject[] _hitEffects;
    private int _currentIndex = 0;
    private float _resetCoolTime = 0.5f;

    private AudioSource _audioSource;

    private void Awake()
    {
        _hitEffects = new GameObject[_count];
        for (int i = 0; i < _count; i++)
        {
            _hitEffects[i] = Instantiate(_hitEffect);
            _hitEffects[i].transform.parent = transform;
            _hitEffects[i].SetActive(false);
        }
        _audioSource = GetComponent<AudioSource>();
    }

    public void Hit(Vector3 hitPoint)
    {
        Debug.Log("피격당함");
        photonView.RPC(nameof(PlayEffect), RpcTarget.All, hitPoint);
    }

    //배열 사용해서 오브젝트 풀링
    [PunRPC]
    private void PlayEffect(Vector3 hitPoint)
    {
        Debug.Log("이펙트 재생");
        //hitPoint 위치에 효과를 재생시키면 된다
        _hitEffects[_currentIndex].transform.position = hitPoint;
        _hitEffects[_currentIndex].SetActive(true);
        _audioSource.Play();
        StartCoroutine(EffectReset(_currentIndex));

        ++_currentIndex;

        if (_currentIndex == _count)
        {
            _currentIndex = 0;
        }
    }

    IEnumerator EffectReset(int index)
    {
        yield return _resetCoolTime;
        _hitEffects[index].SetActive(false);
    }
}
