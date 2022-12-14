using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIdle : AIState
{
    [Header("내 캐릭터 컨트롤러의 콜라이더를 넣어주세요")]
    [SerializeField] private Collider _myCollider;

    private float _curTime;
    private bool _isRunTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _curTime -= _curTime;
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

        // 일정 시간 뒤 Run으로 상태 변경
        if (_curTime >= 4f)
        {
            _aiFSM.ChangeState(EAIState.Run);
            _curTime -= _curTime;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isIdle, false);
        _curTime -= _curTime;
        _isRunTime = false;
    }
}
