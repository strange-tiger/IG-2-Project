using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIFSM : MonoBehaviour
{ 
    private Dictionary<EAIState, AIState> _dictionaryAIState;
    AIState curState = null;

    // ���¸� ��Ƶ� ��ųʸ� �Ҵ�
    public void Init()
    {
        _dictionaryAIState = new Dictionary<EAIState, AIState>();
    }

    public void Update()
    {
        curState.OnUpdate();
    }

    /// <summary>
    /// �����߰� �޼���
    /// </summary>
    /// <param name="tag">Defines.Estate</param>
    /// <param name="aiState">AIState �� �ְ���� ����</param>
    public void AddState(EAIState tag, AIState aiState)
    {
        if (aiState == null)
        {
            Debug.LogError("FSM ����");
        }
        
        aiState.Initialize(this);
        _dictionaryAIState[tag] = aiState;
    }

    /// <summary>
    /// ���� ��ȭ �޼���
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

    // ����Ƽ�� ��ü�߰��� �޼���
    internal void ChangeState(object run)
    {
        throw new NotImplementedException();
    }
}
