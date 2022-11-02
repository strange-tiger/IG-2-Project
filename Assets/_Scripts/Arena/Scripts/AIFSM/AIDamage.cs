using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIDamage : AIState
{
    [Header("���� ���� �������� ���� ��ũ��Ʈ�� �־��ּ���")]
    [SerializeField] private AIAttack _enemyDamage;

    [Header("HP�� �Է� �� �ּ���")]
    [SerializeField] private int _hp;
    public int Hp { get { return _hp; } set { _hp = value; }}

    private int _setHp;

    private Animator _animator;

    private int _damage;

    private float _damageTime;
    private bool _isdamage;

    private void Awake()
    {
        _setHp = _hp;
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _hp = _setHp;
        _enemyDamage.EnemyDamage.AddListener(Hit);
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isDamage, true);

        _hp -= _damage;

        _damageTime -= _damageTime;

        _isdamage = true;
    }

    public override void OnUpdate()
    {
        if (_isdamage)
        {
            _damageTime += Time.deltaTime;
        }

        if (_damageTime >= 0.5f)
        {
            _damageTime -= _damageTime;
            _isdamage = false;
            _animator.SetBool(AIAnimatorID.isDamage, false);
            aiFSM.ChangeState(EAIState.Attack);
        }
        //if (_hp <= 0)
        //{
        //    aiFSM.ChangeState(EAIState.Death);
        //}


    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isDamage, false);
    }

    private void OnDisable()
    {
        _enemyDamage.EnemyDamage.RemoveListener(Hit);
    }

    private void Hit(int damage)
    {
        _damage = damage;
    }
}
