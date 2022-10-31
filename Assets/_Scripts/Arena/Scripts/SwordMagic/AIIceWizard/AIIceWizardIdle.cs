using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIceWizardIdle : AIState
{
    private Animator _animator;

    [Header("내 인식범위 콜라이더를 넣어주세요")]
    [SerializeField] private Collider _myCollider;


    private float _curTime;
    private bool _isRunTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        if (_myCollider.enabled == false)
        {
            _myCollider.enabled = true;
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
