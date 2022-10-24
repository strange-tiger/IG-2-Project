using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AI : MonoBehaviour
{
    AIFSM _aiFSM;

    private int hp;
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
        }    
    }

    private void Awake()
    {
        hp = 100;

        _aiFSM = GetComponent<AIFSM>();
        _aiFSM.Init();
        _aiFSM.AddState(EAIState.IDLE, GetComponent<AIIdel>());
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
