using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerAttack : AIState
{
    public UnityEvent<int> EnemyDamage;

    [SerializeField]
    private AIInfo[] _aIInfo;

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
            Debug.Log("Ä®¸ÂÀ½");
            int _enemyDamage;
            _enemyDamage = other.gameObject.GetComponent<AIInfo>().Damage;
            EnemyDamage.Invoke(_enemyDamage);

            _changeStateAttackToDamage = true;
        }
    }
}
