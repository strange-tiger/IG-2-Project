using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIFSM : MonoBehaviour
{ 
    private Dictionary<EAIState, AIState> _dictionaryAIState;
    AIState curState = null;

    // 상태를 담아둘 딕셔너리 할당
    public void Init()
    {
        _dictionaryAIState = new Dictionary<EAIState, AIState>();
    }

    public void Update()
    {
        curState.OnUpdate();
    }

    /// <summary>
    /// 상태추가 메서드
    /// </summary>
    /// <param name="tag">Defines.Estate</param>
    /// <param name="aiState">AIState 로 넣고싶은 상태</param>
    public void AddState(EAIState tag, AIState aiState)
    {
        if (aiState == null)
        {
            Debug.LogError("FSM 오류");
        }
        
        aiState.Initialize(this);
        _dictionaryAIState[tag] = aiState;
    }

    /// <summary>
    /// 상태 변화 메서드
    /// </summary>
    /// <param name="tag">Defines.Estate</param>
    public void ChangeState(EAIState tag)
    {
        if (curState != null)
        {
            curState.OnExit();
        }
        
        curState = _dictionaryAIState[tag];
        curState.OnEnter();
    }

    // 유니티가 자체추가한 메서드
    internal void ChangeState(object run)
    {
        throw new NotImplementedException();
    }
}
