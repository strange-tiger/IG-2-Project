using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EAttack = Defines.EAttackKind;

public class AIAttack : AIState
{
    [SerializeField]
    private EAttack _eAttack;

    [SerializeField]
    private TriggerDetector _attackAI;

    private Animator _animator;

    private bool _isAttackTime;
    private float _curTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _attackAI._attackAI.RemoveListener(StateChangeAttackToDamage);
        _attackAI._attackAI.AddListener(StateChangeAttackToDamage);

        _attackAI._killAI.RemoveListener(StateChangeAttackToRun);
        _attackAI._killAI.AddListener(StateChangeAttackToRun);

    }

    public override void OnEnter()
    {
        if ((int)_eAttack == 0)
        {
            _animator.SetBool(AIAnimatorID.isAttack1, true);
        }

        else if ((int)_eAttack == 1)
        {
            _animator.SetBool(AIAnimatorID.isAttack2, true);
        }

        _isAttackTime = true;

        _curTime -= _curTime;

    }

    public override void OnUpdate()
    {
        if (_isAttackTime)
        {
            _curTime += Time.deltaTime;
        }

        //if (_curTime >= 5f)
        //{
        //    aiFSM.ChangeState(EAIState.Skill);
        //    _curTime -= _curTime;
        //}
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack1, false);
        _animator.SetBool(AIAnimatorID.isAttack2, false);
        _isAttackTime = false;
        _curTime -= _curTime;
    }

    private void StateChangeAttackToDamage()
    {
        aiFSM.ChangeState(EAIState.Damage);
    }

    private void StateChangeAttackToRun()
    {
        aiFSM.ChangeState(EAIState.IDLE);
    }
}
