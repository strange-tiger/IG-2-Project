using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;


enum EAttackOrder
{
    Attack1_First,
    Attack2_First,
}

public class AIAttack : AIState
{
    [SerializeField]
    private EAttackOrder _eAttackOrder;

    private Animator _animator;
    private AI _ai;

    private bool _isAttackTime;
    private float _curTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _ai = GetComponent<AI>();


    }

    public override void OnEnter()
    {

        if (_eAttackOrder == EAttackOrder.Attack1_First)
        {
            _animator.SetBool(AIAnimatorID.isAttack1, true);
            Debug.Log("OnEnter AIAIAttack1");
        }

        else if (_eAttackOrder == EAttackOrder.Attack2_First)
        {
            _animator.SetBool(AIAnimatorID.isAttack2, true);
            Debug.Log("OnEnter AIAIAttack2");
        }

        _isAttackTime = true;
    }

    public override void OnUpdate()
    {
        if (_isAttackTime)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 5f)
        {
            aiFSM.ChangeState(EAIState.Skill);
            _curTime -= _curTime;
        }

        if (_ai.HP <= 0)
        {
            aiFSM.ChangeState(EAIState.Death);
        }

    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack1, false);
        _animator.SetBool(AIAnimatorID.isAttack2, false);
        _isAttackTime = false;
        _curTime -= _curTime;
    }


}
