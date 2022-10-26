using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJob = Defines.EJobClass;

public class AI : MonoBehaviour
{
    [SerializeField]
    private EJob _eJob;

    AIFSM _aiFSM;
    private Animator _animator;
    private Transform _transform;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _aiFSM = GetComponent<AIFSM>();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, GetComponent<AIIdle>());
        _aiFSM.AddState(EAIState.Run, GetComponent<AIRun>());
        _aiFSM.AddState(EAIState.Attack, GetComponent<AIAttack>());
        _aiFSM.AddState(EAIState.Skill, GetComponent<AISkill>());
        _aiFSM.AddState(EAIState.Damage, GetComponent<AIDamage>());
        _aiFSM.AddState(EAIState.Death, GetComponent<AIDeath>());
        _aiFSM.ChangeState(EAIState.IDLE);
    }

    private void Update()
    {
        _aiFSM.Update();
    }
}
