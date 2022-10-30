using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerDamage : AIState
{
    [SerializeField] private AIHighClassAdventurerAttack _aIHighClassAdventurerAttack;

    [Header("HP를 입력 해 주세요")]
    [SerializeField] private int _hp;

    private Animator _animator;
    private int _curHP;
    private int _damage;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _aIHighClassAdventurerAttack.EnemyDamage.AddListener(Hit);
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerDamage, true);

        _curHP -= _damage;

    }

    public override void OnUpdate()
    {
        if (_curHP <= 0)
        {
            aiFSM.ChangeState(EAIState.Death);
        }
        else
        {
            aiFSM.ChangeState(EAIState.Attack);
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerDamage, false);
    }

    private void Hit(int damage)
    {
        _hp -= damage;
    }

}
