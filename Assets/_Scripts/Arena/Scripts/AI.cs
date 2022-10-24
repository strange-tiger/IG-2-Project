using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AI : MonoBehaviour
{
    AIFSM _aiFSM;

    private void Awake()
    {
        _aiFSM = new AIFSM();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, new AIIdel());
        _aiFSM.AddState(EAIState.Run, new AIRun());
        _aiFSM.AddState(EAIState.Attack, new AIAttack());
        _aiFSM.AddState(EAIState.Damage, new AIDamage());
        _aiFSM.AddState(EAIState.Death, new AIDeath());
        _aiFSM.ChangeState(EAIState.IDLE);
    }

    private void Update()
    {
        _aiFSM.Update();
    }
}
