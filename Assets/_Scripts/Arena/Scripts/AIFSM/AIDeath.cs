using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIDeath : AIState
{
    [SerializeField]
    private Collider[] _isDeathOffCollider;

    private Animator _animator;


    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        Invoke("OffCollider", 1f);

        _animator.SetTrigger(AIAnimatorID.onDeath);

        Invoke("Delete", 10f);

        Debug.Log($"{gameObject.name} : Death OnEnter");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDeath, false);
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
