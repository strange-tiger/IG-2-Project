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

        // 죽음상태에 들어온 후 다음 토너먼트 진행을 위한 시체 제거
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
