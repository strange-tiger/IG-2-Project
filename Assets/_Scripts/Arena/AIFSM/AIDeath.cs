using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIDeath : AIState
{
    [SerializeField] private Collider[] _isDeathOffCollider;

    [SerializeField] private AudioClip _deathAudioClip;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public override void OnEnter()
    {
        if (_deathAudioClip != null)
        {
            _audioSource.PlayOneShot(_deathAudioClip);
        }

        Invoke("OffCollider", 1f);

        _animator.SetTrigger(AIAnimatorID.onDeath);

        // �������¿� ���� �� ���� ��ʸ�Ʈ ������ ���� ��ü ����
        Invoke("Delete", 5f);


    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        
    }

    private void Delete()
    {
        gameObject.SetActive(false);
    }
    private void OffCollider()
    {
        for (int i = 0; i < _isDeathOffCollider.Length; ++i)
        {
            _isDeathOffCollider[i].enabled = false;
        }
    }
}
