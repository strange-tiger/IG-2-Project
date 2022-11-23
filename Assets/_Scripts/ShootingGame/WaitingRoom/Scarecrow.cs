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
        Debug.Log("�ǰݴ���");
        photonView.RPC(nameof(PlayEffect), RpcTarget.All, hitPoint);
    }

    //�迭 ����ؼ� ������Ʈ Ǯ��
    [PunRPC]
    private void PlayEffect(Vector3 hitPoint)
    {
        Debug.Log("����Ʈ ���");
        //hitPoint ��ġ�� ȿ���� �����Ű�� �ȴ�
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
