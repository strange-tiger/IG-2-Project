using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class AIAttack : AIState
{
    [Header("내 스킬 쿨타임을 넣어주세요")]
    [SerializeField] int _skillCoolTime;

    [SerializeField] private AudioClip _attackAudioClip;

    public UnityEvent<int> EnemyDamage;
    public UnityEvent<int> EnemySkillDamage;

    private bool _isSkillCoolTime;
    private bool _changeStateAttackToDamage;
    private bool _isAnd;
    private float _curTime;

    private int _myDamage;
    private int _enemyDamage;

    private int _mySkillDamage;
    private int _enemySkillDamage;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        EnemyDamage = new UnityEvent<int>();
        EnemySkillDamage = new UnityEvent<int>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isAttack, true);
        _isSkillCoolTime = true;
        _changeStateAttackToDamage = false;
        _isAnd = false;

        if (_attackAudioClip != null)
        {
            _audioSource.PlayOneShot(_attackAudioClip);
        }
    }

    public override void OnUpdate()
    {
        if (_changeStateAttackToDamage == true)
        {
            _aiFSM.ChangeState(EAIState.Damage);
        }

        if (_isSkillCoolTime == true && _skillCoolTime != 0 && !_isAnd)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= _skillCoolTime)
        {
            _curTime -= _curTime;
            _isSkillCoolTime = false;
            _aiFSM.ChangeState(EAIState.Skill);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack, false);
        _isSkillCoolTime = false;
        _changeStateAttackToDamage = false;
    }

    // 무기 혹은 스킬에 맞으면
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "AIRange")
        {
            int _enemyHp;
            _enemyHp = other.gameObject.GetComponentInParent<AIDamage>().Hp;

            if (_enemyHp <= 0)
            {
                _animator.SetBool(AIAnimatorID.isAttack, false);
                _animator.SetBool(AIAnimatorID.isIdle, true);
                _isAnd = true;
            }
        }
    }

    /// <summary>
    /// 때린얘의 데미지를 가져와 이벤트 실행
    /// </summary>
    /// <param name="other"></param>
    private void AttackProcess(Collider other)
    {
        _enemyDamage = other.gameObject.GetComponentInParent<AIInfo>().Damage;
        EnemyDamage.Invoke(_enemyDamage);

        _changeStateAttackToDamage = true;
    }

    /// <summary>
    /// 때린얘의 스킬 데미지를 가져와 이벤트 실행
    /// </summary>
    /// <param name="other"></param>
    private void SkillProcess(Collider other)
    {
        _enemySkillDamage = other.gameObject.GetComponentInParent<AIInfo>().SkillDamage;
        EnemySkillDamage.Invoke(_enemySkillDamage);

        _changeStateAttackToDamage = true;
    }
}
