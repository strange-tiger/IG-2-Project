using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AISkill : AIState
{
    [Header("내 스킬 이펙트을 넣어주세요")]
    [SerializeField] GameObject _skillEffect;

    private Animator _animator;

    private float _curTime;
    private bool _isSkill;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _isSkill = true;
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isSkill, true);
        if (_skillEffect != null)
        {
            _skillEffect.SetActive(true);
        }
        
        _isSkill = true;
    }

    public override void OnUpdate()
    {
        if (_isSkill == true)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 2f)
        {
            aiFSM.ChangeState(EAIState.Attack);
            _isSkill = false;
            _curTime -= _curTime;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isSkill, false);
        if (_skillEffect != null)
        {
            _skillEffect.SetActive(false);
        }
        
    }
}
