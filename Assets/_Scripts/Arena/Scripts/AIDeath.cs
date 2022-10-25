using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDeath : AIState
{
    [SerializeField]
    private Collider[] _collider;

    private Animator _animator;

    private bool _isDie;
    private float _curTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isDeath, true);

        _collider[0].enabled = false;
        _collider[1].enabled = false;

        _curTime -= _curTime;
        _isDie = true;
    }

    public override void OnUpdate()
    {
        if (_isDie)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime > 2f)
        {
            OnExit();
            _curTime -= _curTime;
            _isDie = false;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDeath, false);
    }
}
