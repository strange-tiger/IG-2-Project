using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeath : AIState
{
    [SerializeField]
    private Collider _collider;

    private Animator _animator;

    

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isDeath, true);
        _collider.enabled = false;
    }

    public override void OnUpdate()
    {
        // 경기가 끝날때까지 
    }

    public override void OnExit()
    {
        // 
    }
    
}
