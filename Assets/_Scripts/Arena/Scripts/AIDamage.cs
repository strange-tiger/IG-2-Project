using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIDamage : AIState
{
    private AI _ai;
    private Animator _animator;

    private int _damage = 5;

    private void Start()
    {
        _ai = GetComponent<AI>();
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        Debug.Log("OnEnter Damage");
        _animator.SetBool(AIAnimatorID.isDamage, true);
        _ai.HP -= _damage;
    }

    public override void OnUpdate()
    {
        if (_ai.HP <= 0)
        {
            aiFSM.ChangeState(EAIState.Death);
        }
        else
        {
            Debug.Log("OnUpdate Attack");
            aiFSM.ChangeState(EAIState.Attack);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDamage, false);
        Debug.Log("OnExit Damage");
    }
}
