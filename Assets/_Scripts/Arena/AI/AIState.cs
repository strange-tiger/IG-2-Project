using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState : MonoBehaviour
{
    protected AIFSM aiFSM;

    protected Animator _animator;
    protected AudioSource _audioSource; 

    public void Initialize(AIFSM aiFSM)
    {
        this.aiFSM = aiFSM;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
}
