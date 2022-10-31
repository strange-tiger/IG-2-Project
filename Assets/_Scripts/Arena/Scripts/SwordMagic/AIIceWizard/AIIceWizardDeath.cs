using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIceWizardDeath : AIState
{
    [SerializeField]
    private Collider _isDeathOffCollider;

    private Animator _animator;

    private bool _isDeath;
    private float _deathTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _isDeathOffCollider.enabled = false;

        _animator.SetBool(AIAnimatorID.isDeath, true);

        _deathTime -= _deathTime;
        _isDeath = true;
    }

    public override void OnUpdate()
    {
        if (_isDeath)
        {
            _deathTime += Time.deltaTime;
        }

        if (_deathTime >= 3.5f)
        {
            //_animator.SetBool(AIAnimatorID.isIceWizardDeath, false);
            _isDeath = false;

            //aiFSM.ChangeState(EAIState.IDLE);
            gameObject.SetActive(false);
            _deathTime -= _deathTime;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDeath, false);
    }

    private void OnDisable()
    {
        aiFSM.ChangeState(EAIState.IDLE);
    }
}
