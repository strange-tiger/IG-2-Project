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
        _triggerDetector._onSword.AddListener(SwordTouchMyBody);
    }

    public override void OnEnter()
    {
        
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
    
    private void SwordTouchMyBody()
    {
        _animator.SetTrigger(AIAnimatorID.onDamage);
        _ai.HP -= _damage;

        if (_ai.HP <= 0)
        {
            aiFSM.ChangeState(EAIState.Death);
        }
    }
}
