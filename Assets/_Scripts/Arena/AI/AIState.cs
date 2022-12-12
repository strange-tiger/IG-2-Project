using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    protected AIFSM _aiFSM;

    protected Animator _animator;
    protected AudioSource _audioSource; 

    public void Initialize(AIFSM aiFSM)
    {
        this._aiFSM = aiFSM;
    }

    // 해당 상태에 들어왔을 때 실행
    public abstract void OnEnter();
    // 해당 상태에 업데이트
    public abstract void OnUpdate();
    // 해당 상태에서 벗어날 때 실행
    public abstract void OnExit();
}
