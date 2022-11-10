using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJob = Defines.EJobClass;
using Photon.Pun;

public class AI : MonoBehaviour
{
    AIFSM _aiFSM;

    private void OnEnable()
    {
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
