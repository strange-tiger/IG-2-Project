using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJob = Defines.EJobClass;

public class AIFireWizard : MonoBehaviour
{
    AIFSM _aiFSM;

    private void OnEnable()
    {
        _aiFSM = GetComponent<AIFSM>();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, GetComponent<AIFireWizardIdle>());
        _aiFSM.AddState(EAIState.Run, GetComponent<AIFireWizardRun>());
        _aiFSM.AddState(EAIState.Attack, GetComponent<AIFireWizardAttack>());
        _aiFSM.AddState(EAIState.Skill, GetComponent<AIFireWizardSkill>());
        _aiFSM.AddState(EAIState.Damage, GetComponent<AIFireWizardDamage>());
        _aiFSM.AddState(EAIState.Death, GetComponent<AIFireWizardDeath>());
        _aiFSM.ChangeState(EAIState.IDLE);
    }

    private void Update()
    {
        _aiFSM.Update();
    }
}
