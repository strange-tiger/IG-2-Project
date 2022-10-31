using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJob = Defines.EJobClass;

public class AIHighClassKnight : MonoBehaviour
{
    AIFSM _aiFSM;

    private void OnEnable()
    {
        _aiFSM = GetComponent<AIFSM>();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, GetComponent<AIHighClassKnightIdle>());
        _aiFSM.AddState(EAIState.Run, GetComponent<AIHighClassKnightRun>());
        _aiFSM.AddState(EAIState.Attack, GetComponent<AIHighClassKnightAttack>());
        _aiFSM.AddState(EAIState.Skill, GetComponent<AIHighClassKnightSkill>());
        _aiFSM.AddState(EAIState.Damage, GetComponent<AIHighClassKnightDamage>());
        _aiFSM.AddState(EAIState.Death, GetComponent<AIHighClassKnightDeath>());
        _aiFSM.ChangeState(EAIState.IDLE);
    }

    private void Update()
    {
        _aiFSM.Update();
    }
}
