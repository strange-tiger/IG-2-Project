using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EAttack = Defines.EAttackKind;

public class AIDamage : AIState
{
    [SerializeField]
    private EAttack _eAttack;

    private Animator _animator;
    private AI _ai;

    private int _damage = 10;

    private bool _isDamageTime;
    private float _curTime;

    private void Start()
    {
        _ai = GetComponent<AI>();
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isDamage, true);

        _ai.HP -= _damage;

        _isDamageTime = true;
    }

    public override void OnUpdate()
    {
        if (_ai.HP <= 0)
        {
            _isDamageTime = false;
            _curTime -= _curTime;
            aiFSM.ChangeState(EAIState.Death);
            Debug.Log("17 AIDamage Die // ав╬Н╤Ы");
        }

        if (_isDamageTime)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 0.2f)
        {
            _isDamageTime = false;
            _curTime -= _curTime;
            aiFSM.ChangeState(EAIState.Attack);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDamage, false);
    }
}
