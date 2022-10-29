using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerAttack : AIState
{
    private Animator _animator;

    private bool _isSkillCoolTime;
    private bool _changeStateAttackToDamage;
    private float _curTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerAttack, true);
    }

    public override void OnUpdate()
    {
        if (_changeStateAttackToDamage == true)
        {
            aiFSM.ChangeState(EAIState.Damage);

            _changeStateAttackToDamage = false;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerAttack, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            _changeStateAttackToDamage = true;
        }
    }
}
