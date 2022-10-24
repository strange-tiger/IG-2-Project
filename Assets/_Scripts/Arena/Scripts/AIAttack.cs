using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        if (_eAttackOrder == EAttackOrder.Attack1_First)
        {
            _animator.SetBool(AIAnimatorID.isAttack1, true);
        }

        else if (_eAttackOrder == EAttackOrder.Attack2_First)
        {
            _animator.SetBool(AIAnimatorID.isAttack2, true);
        }
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack1, false);
        _animator.SetBool(AIAnimatorID.isAttack2, false);
    }

    
}
