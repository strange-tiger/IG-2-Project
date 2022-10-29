using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHighClassKnightDeath : AIState
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
        _isDeathOffCollider[0].enabled = false;
        _isDeathOffCollider[1].enabled = false;

        _animator.SetBool(AIAnimatorID.isHighClassKnightDeath, true);
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isHighClassKnightDeath, false);
    }
}
