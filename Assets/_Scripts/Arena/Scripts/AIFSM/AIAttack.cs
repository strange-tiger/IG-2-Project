using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class AIAttack : AIState
{
    [SerializeField] private  CharacterController _characterController;

    [Header("내 스킬 쿨타임을 넣어주세요")]
    [SerializeField] int _skillCoolTime;

    public UnityEvent<int> EnemyDamage;


    private Animator _animator;

    private bool _isSkillCoolTime;
    private bool _changeStateAttackToDamage;
    private bool _isAnd;
    private float _curTime;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        EnemyDamage = new UnityEvent<int>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isAttack, true);
        _isSkillCoolTime = true;
        _changeStateAttackToDamage = false;
    }

    public override void OnUpdate()
    {
        if (_changeStateAttackToDamage == true)
        {
            aiFSM.ChangeState(EAIState.Damage);

            _changeStateAttackToDamage = false;
        }

        if (_isSkillCoolTime == true && _skillCoolTime != 0 && !_isAnd)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= _skillCoolTime)
        {
            _curTime -= _curTime;
            _isSkillCoolTime = false;
            aiFSM.ChangeState(EAIState.Skill);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack, false);
        _isSkillCoolTime = false;
        _changeStateAttackToDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            AttackProcess(other);
        }

        if (other.gameObject.tag == "AIMagic")
        {
            SkillProcess(other);
        }
    }

    private void AttackProcess(Collider other)
    {
        int _enemyDamage;
        _enemyDamage = other.gameObject.GetComponentInParent<AIInfo>().Damage;
        EnemyDamage.Invoke(_enemyDamage);

        int _myHp;
        _myHp = GetComponent<AIDamage>().Hp;
        //Debug.Log($"나 : {gameObject.name} 적 : {other.gameObject.name} 내 Hp : {_myHp}");

        int _myDamage;
        _myDamage = GetComponent<AIInfo>().Damage;

        int _enemyHp;
        _enemyHp = other.gameObject.GetComponentInParent<AIDamage>().Hp;

        if (_myHp <= _enemyDamage)
        {
            aiFSM.ChangeState(EAIState.Death);

            Destroy(_characterController);
            
            // gameObject.SetActive(false);

            Debug.Log($"{gameObject.name} : 전이제 죽습니다");
        }

        if (_enemyHp <= _myDamage)
        {
            _animator.SetBool(AIAnimatorID.isAttack, false);
            _animator.SetBool(AIAnimatorID.isIdle, true);
            _isAnd = true;
            // aiFSM.ChangeState(EAIState.IDLE);

            Debug.Log($"{gameObject.name} : 승리!");
        }

        _changeStateAttackToDamage = true;
    }

    private void SkillProcess(Collider other)
    {
        int _enemyDamage;
        _enemyDamage = other.gameObject.GetComponentInParent<AIInfo>().SkillDamage;
        EnemyDamage.Invoke(_enemyDamage);

        int _myHp;
        _myHp = GetComponent<AIDamage>().Hp;
        //Debug.Log($"나 : {gameObject.name} 적 : {other.gameObject.name} 내 Hp : {_myHp}");

        int _myDamage;
        _myDamage = GetComponent<AIInfo>().SkillDamage;

        int _enemyHp;
        _enemyHp = other.gameObject.GetComponentInParent<AIDamage>().Hp;

        if (_myHp <= _enemyDamage)
        {
            aiFSM.ChangeState(EAIState.Death);

            Destroy(_characterController);

            // gameObject.SetActive(false);

            Debug.Log($"{gameObject.name} : 전이제 죽습니다");
        }

        if (_enemyHp <= _myDamage)
        {
            _animator.SetBool(AIAnimatorID.isAttack, false);
            _animator.SetBool(AIAnimatorID.isIdle, true);
            _isAnd = true;
            // aiFSM.ChangeState(EAIState.IDLE);

            Debug.Log($"{gameObject.name} : 승리!");
        }

        _changeStateAttackToDamage = true;
    }
}
