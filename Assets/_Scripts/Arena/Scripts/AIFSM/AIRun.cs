using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIRun : AIState
{
    [SerializeField] private Collider _myCollider;

    private Animator _animator;

    private bool _changeStateRunToAttack;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _changeStateRunToAttack = false;
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isRun, true);
    }

    public override void OnUpdate()
    {
        if (_changeStateRunToAttack == true)
        {
            aiFSM.ChangeState(EAIState.Attack);

            _changeStateRunToAttack = false;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isRun, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AIRange")
        {
            // _myCollider.enabled = false;
            _changeStateRunToAttack = true;
        }
    }
}
