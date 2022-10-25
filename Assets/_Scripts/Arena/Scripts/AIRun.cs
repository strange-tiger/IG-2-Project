using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIRun : AIState
{
    private Animator _animator;

    private float _curTime;
    private bool _isRunTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        Debug.Log("OnEnter AIRun");
        _isRunTime = true;
        _animator.SetBool(AIAnimatorID.isRun, true);
    }

    public override void OnUpdate()
    {
        if (_isRunTime)
        {
            _curTime += Time.deltaTime;
        }

        if (_curTime >= 3f)
        {
            int a = Random.Range(0, 361);
            transform.Rotate(new Vector3(0, a, 0));
            _curTime -= _curTime;
        }
    }

    public override void OnExit()
    {
        Debug.Log("OnExit AIRun");
        _animator.SetBool(AIAnimatorID.isRun, false);
        _isRunTime = false;
        _curTime -= _curTime;
    }
}
