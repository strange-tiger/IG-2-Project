using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIDeath : AIState
{
    [SerializeField]
    private Collider[] _isDeathOffCollider;

    public override void OnEnter()
    {
        Invoke("OffCollider", 1f);

        _animator.SetTrigger(AIAnimatorID.onDeath);

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
