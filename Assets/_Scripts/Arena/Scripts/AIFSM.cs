using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIFSM : MonoBehaviour
{ 
    private Dictionary<EAIState, AIState> _dictionaryAIState;
    AIState curState = null;

    public void Init()
    {
        _dictionaryAIState = new Dictionary<EAIState, AIState>();
    }

    public void Update()
    {
        curState.OnUpdate();
    }

    public void AddState(EAIState tag, AIState aiState)
    {
        if (aiState == null)
        {
            Debug.LogError("FSM 오류");
        }
        Debug.Log("AddState 실행");
        aiState.Initialize(this);
        _dictionaryAIState[tag] = aiState;
    }

    public void ChangeState(EAIState tag)
    {
        if (curState != null)
        {
            curState.OnExit();
        }
        Debug.Log("ChangeState 실행");
        curState = _dictionaryAIState[tag];
        curState.OnEnter();
    }

    internal void ChangeState(object run)
    {
        throw new NotImplementedException();
    }
}
