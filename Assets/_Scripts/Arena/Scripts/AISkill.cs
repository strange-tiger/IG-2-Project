using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AISkill : AIState
{
    private Animator _animator;
    private AI _ai;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _ai = GetComponent<AI>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isSkill, true);
    }

    public override void OnUpdate()
    {
        if (_ai.HP <= 0)
        {
            aiFSM.ChangeState(EAIState.Attack);
        }
        else
        {
            aiFSM.ChangeState(EAIState.Death);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isSkill, false);
    }

}
