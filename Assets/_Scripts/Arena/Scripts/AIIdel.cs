using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIdel : AIState
{
    private Animator _animator;
    private float curTime;
    private bool isRunTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        isRunTime = true;
    }

    private void Update()
    {
        
    }

    public override void OnEnter()
    {

    }

    public override void OnUpdate()
    {
        if (isRunTime)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= 2f)
        {
            Debug.Log("2√ ¡ˆ≥≤");
            aiFSM.ChangeState(EAIState.Run);
            curTime -= curTime;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isIdel, false);
        isRunTime = false;
    }
}