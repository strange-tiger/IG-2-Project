using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJob = Defines.EJobClass;

public class AIIceWizard : MonoBehaviour
{
    AIFSM _aiFSM;

    private void OnEnable()
    {
        _aiFSM = GetComponent<AIFSM>();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, GetComponent<AIIceWizardIdle>());
        _aiFSM.AddState(EAIState.Run, GetComponent<AIIceWizardRun>());
        _aiFSM.AddState(EAIState.Attack, GetComponent<AIIceWizardAttack>());
        _aiFSM.AddState(EAIState.Skill, GetComponent<AIIceWizardSkill>());
        _aiFSM.AddState(EAIState.Damage, GetComponent<AIIceWizardDamage>());
        _aiFSM.AddState(EAIState.Death, GetComponent<AIIceWizardDeath>());
        _aiFSM.ChangeState(EAIState.IDLE);
    }

    void Update()
    {
        
    }
}
