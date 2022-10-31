using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJob = Defines.EJobClass;

public class AIHighClassAdventurer : MonoBehaviour
{
    [SerializeField]
    private EJob _eClass;

    AIFSM _aiFSM;
    private Animator _animator;
    private Transform _transform;


    private int _classNumber;
    public int ClassNumber
    {
        get
        {
            return _classNumber;
        }
        set
        {
            _classNumber = value;
        }
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _aiFSM = GetComponent<AIFSM>();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, GetComponent<AIHighClassAdventurerIdle>());
        _aiFSM.AddState(EAIState.Run, GetComponent<AIHighClassAdventurerRun>());
        _aiFSM.AddState(EAIState.Attack, GetComponent<AIHighClassAdventurerAttack>());
        _aiFSM.AddState(EAIState.Skill, GetComponent<AIHighClassAdventurerSkill>());
        _aiFSM.AddState(EAIState.Damage, GetComponent<AIHighClassAdventurerDamage>());
        _aiFSM.AddState(EAIState.Death, GetComponent<AIHighClassAdventurerDeath>());
        _aiFSM.ChangeState(EAIState.IDLE);
    }

    private void Update()
    {
        _aiFSM.Update();
    }
}
