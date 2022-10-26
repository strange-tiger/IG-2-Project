using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIIdel : AIState
{
    [SerializeField]
    private Collider _aiCollider;

    private Animator _animator;
    private float curTime;
    private bool isRunTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        isRunTime = true;

        if (_aiCollider.enabled == false)
        {
            _aiCollider.enabled = true;
        }
        
    }

    public override void OnUpdate()
    {
        if (isRunTime)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= 2f)
        {
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