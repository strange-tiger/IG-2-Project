using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIdle : AIState
{
    [Header("�� ĳ���� ��Ʈ�ѷ��� �ݶ��̴��� �־��ּ���")]
    [SerializeField] private Collider _myCollider;

    private float _curTime;
    private bool _isRunTime;

    private void OnEnable()
    {
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

        if (_curTime >= 4f)
        {
            aiFSM.ChangeState(EAIState.Run);
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
