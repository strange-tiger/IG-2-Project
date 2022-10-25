using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeath : AIState
{
    [SerializeField]
    private Collider[] _collider;

    private Animator _animator;

    private AI _ai;
    private bool _isDie;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _ai = GetComponent<AI>();
    }

    public override void OnEnter()
    {
        
        _animator.SetBool(AIAnimatorID.isDeath, true);

        _collider[0].enabled = false;
        _collider[1].enabled = false;

        _isDie = true;
    }

    public override void OnUpdate()
    {
        if (_isDie)
        {
            _animator.SetBool(AIAnimatorID.isDeath, false);
            _isDie = false;
        }
    }

    public override void OnExit()
    {
        
    }
    
}
