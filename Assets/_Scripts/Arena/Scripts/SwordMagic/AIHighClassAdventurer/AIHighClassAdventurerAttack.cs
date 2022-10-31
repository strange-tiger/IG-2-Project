using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerAttack : AIState
{
    [Header("상대방의 스크립트를 넣어주세요")]
    [SerializeField] AIInfo[] _aIInfo;

    public UnityEvent<int> EnemyDamage;

    private Animator _animator;

    private bool _isSkillCoolTime;
    private bool _changeStateAttackToDamage;
    private float _curTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        EnemyDamage = new UnityEvent<int>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isAttack, true);
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
        _animator.SetBool(AIAnimatorID.isAttack, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            int _enemyDamage;
            _enemyDamage = other.gameObject.GetComponentInParent<AIInfo>().Damage;
            EnemyDamage.Invoke(_enemyDamage);
            _changeStateAttackToDamage = true;
        }
    }
}
