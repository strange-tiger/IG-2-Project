using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EAttack = Defines.EAttackKind;

public class AIAttack : AIState
{
    [SerializeField]
    private EAttack _eAttack;

    [SerializeField]
    private TriggerDetector _attackOne;

    [SerializeField]
    private TriggerDetector _attackTwo;

    [SerializeField]
    private TriggerDetector _attackThree;

    [SerializeField]
    private AIDeath _killAIOne;

    [SerializeField]
    private AIDeath _killAITwo;

    [SerializeField]
    private AIDeath _killAIThree;

    private Animator _animator;

    private bool _isAttackTime;
    private float _curTime;
    private int _damage;
    public int Damage { get { return _damage; } set { _damage = value; } }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _attackOne.AttackAI.RemoveListener(StateChangeAttackToDamage);
        _attackOne.AttackAI.AddListener(StateChangeAttackToDamage);

        _attackTwo.AttackAI.RemoveListener(StateChangeAttackToDamage);
        _attackTwo.AttackAI.AddListener(StateChangeAttackToDamage);

        _attackThree.AttackAI.RemoveListener(StateChangeAttackToDamage);
        _attackThree.AttackAI.AddListener(StateChangeAttackToDamage);

        _killAIOne.KillAI.RemoveListener(StateChangeAttackToRun);
        _killAIOne.KillAI.AddListener(StateChangeAttackToRun);

        _killAITwo.KillAI.RemoveListener(StateChangeAttackToRun);
        _killAITwo.KillAI.AddListener(StateChangeAttackToRun);

        _killAIThree.KillAI.RemoveListener(StateChangeAttackToRun);
        _killAIThree.KillAI.AddListener(StateChangeAttackToRun);
    }

    public override void OnEnter()
    {
        if ((int)_eAttack == 0)
        {
            _animator.SetBool(AIAnimatorID.isAttack1, true);
        }

        else if ((int)_eAttack == 1)
        {
            _animator.SetBool(AIAnimatorID.isAttack2, true);
        }

        _isAttackTime = true;

        _curTime -= _curTime;

    }

    public override void OnUpdate()
    {
        if (_isAttackTime)
        {
            _curTime += Time.deltaTime;
        }

        //if (_curTime >= 5f)
        //{
        //    aiFSM.ChangeState(EAIState.Skill);
        //    _curTime -= _curTime;
        //}
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isAttack1, false);
        _animator.SetBool(AIAnimatorID.isAttack2, false);
        _isAttackTime = false;
        _curTime -= _curTime;
    }

    private void StateChangeAttackToDamage(int damage)
    {
        _damage = damage;
        aiFSM.ChangeState(EAIState.Damage);
    }

    private void StateChangeAttackToRun()
    {
        aiFSM.ChangeState(EAIState.IDLE);
    }

    private void OnDisable()
    {
        _attackOne.AttackAI.RemoveListener(StateChangeAttackToDamage);
        _attackTwo.AttackAI.RemoveListener(StateChangeAttackToDamage);
        _attackThree.AttackAI.RemoveListener(StateChangeAttackToDamage);

        _killAIOne.KillAI.RemoveListener(StateChangeAttackToRun);
        _killAITwo.KillAI.RemoveListener(StateChangeAttackToRun);
        _killAIThree.KillAI.RemoveListener(StateChangeAttackToRun);
    }
}
