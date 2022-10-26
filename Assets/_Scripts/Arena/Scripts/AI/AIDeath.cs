using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AIDeath : AIState
{
    [SerializeField]
    private Collider[] _collider;

    private Animator _animator;
    public UnityEvent KillAI = new UnityEvent();

    private bool _isDie;
    private float _curTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isDeath, true);

        _animator.SetBool(AIAnimatorID.isIdle, false);
        _animator.SetBool(AIAnimatorID.isAttack1, false);
        _animator.SetBool(AIAnimatorID.isAttack2, false);
        _animator.SetBool(AIAnimatorID.isRun, false);

        _collider[0].enabled = false;
        _collider[1].enabled = false;

        KillAI.Invoke();
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
            gameObject.SetActive(false);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDeath, false);
    }
}
