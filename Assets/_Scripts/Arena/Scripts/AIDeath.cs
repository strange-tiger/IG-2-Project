using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeath : AIState
{
    [SerializeField]
    private Collider _collider;

    private Animator _animator;

    private bool _isDie;
    private float _curTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isAttack1, false);
        _animator.SetBool(AIAnimatorID.isAttack2, false);
        _animator.SetBool(AIAnimatorID.isDeath, true);

        _collider.enabled = false;

        _curTime -= _curTime;
        _isDie = true;
    }

    public override void OnUpdate()
    {
        if (_isDie)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 1.5f)
        {
            _animator.SetBool(AIAnimatorID.isDeath, false);
            _curTime -= _curTime;
            _isDie = false;
        }
    }

    public override void OnExit()
    {
        
    }
}
