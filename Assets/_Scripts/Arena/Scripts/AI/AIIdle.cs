using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIdle : AIState
{
    [SerializeField]
    private Collider _aiCollider;

    private Animator _animator;
    private float _curTime;
    private bool _isRunTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _isRunTime = true;
    }

    public override void OnEnter()
    {
        if (_aiCollider.enabled == false)
        {
            _aiCollider.enabled = true;
        }
        _isRunTime = true;
        _curTime -= _curTime;
    }

    public override void OnUpdate()
    {
        if (_isRunTime)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 2f)
        {
            aiFSM.ChangeState(EAIState.Run);
            _curTime -= _curTime;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isIdle, false);
        _isRunTime = false;
    }
}