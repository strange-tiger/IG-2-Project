using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EAttack = Defines.EAttackKind;

public class AIDamage : AIState
{
    [SerializeField]
    private EAttack _eAttack;

    private Animator _animator;
    
    private int Damage = 35;

    private bool _isDamageTime;
    private float _curTime;

    [Header("체력을 입력 해 주세요")]
    [SerializeField]
    private int _hp;

    public int Hp
    {
        get
        {
            return _hp;
        }
        private set
        {
            _hp = value;
        }
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isDamage, true);

        _hp -= Damage;
        Debug.Log($"Damage : {_hp}" );
        _isDamageTime = true;

        
    }

    public override void OnUpdate()
    {
        if (_hp <= 0)
        {
            _isDamageTime = false;
            _curTime -= _curTime;

            aiFSM.ChangeState(EAIState.Death);
        }

        if (_isDamageTime)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime > 1f)
        {
            _isDamageTime = false;
            _curTime -= _curTime;
            aiFSM.ChangeState(EAIState.Attack);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDamage, false);
    }
}
