using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Reflection;

public class Scarecrow : MonoBehaviourPun
{
    [Header("�̸� �����س��� ��ƼŬ")]
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
        Debug.Log("�ǰݴ���");
        photonView.RPC(nameof(PlayEffect), RpcTarget.All, hitPoint);
    }

    [PunRPC]
    private void PlayEffect(Vector3 hitPoint)
    {
        Debug.Log("����Ʈ ���");
        //hitPoint ��ġ�� ȿ���� �����Ű�� �ȴ�
        _hitEffects[_currentIndex].gameObject.SetActive(false);
        _hitEffects[_currentIndex].gameObject.transform.position = hitPoint;
        _hitEffects[_currentIndex].gameObject.SetActive(true);
        _audioSource.Play();

        _currentIndex = (_currentIndex + 1) % _hitEffects.Count;
    }
}
