using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIDamage : AIState
{
    private AI _ai;

    private Animator _animator;

    [SerializeField]
    private TriggerDetector _triggerDetector;

    private int _damage = 10;

    private void Start()
    {
        _ai = GetComponent<AI>();
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
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
            aiFSM.ChangeState(EAIState.Attack);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDamage, false);
    }
    
    private void SwordTouchMyBody()
    {
        aiFSM.ChangeState(EAIState.Damage);
    }
}
