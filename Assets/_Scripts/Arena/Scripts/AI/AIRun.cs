using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIRun : AIState
{
    [SerializeField]
    private TriggerDetector _hiAI;

    private Animator _animator;

    //private float _curTime;
    //private bool _isRunTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _hiAI.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAI.HiAI.AddListener(StateChangeRunToAttack);
    }

    private void OnEnable()
    {
        
    }

    public override void OnEnter()
    {
        //_isRunTime = true;
        _animator.SetBool(AIAnimatorID.isRun, true);
    }

    public override void OnUpdate()
    {
        //if (_isRunTime)
        //{
        //    _curTime += Time.deltaTime;
        //}
        
        //if (_curTime >= 4f)
        //{
        //    int a = Random.Range(0, 361);
        //    transform.Rotate(new Vector3(0, a, 0));
        //    _curTime -= _curTime;
        //}
    }

    public override void OnExit()
    {
        //_isRunTime = false;
        //_curTime -= _curTime;
        _animator.SetBool(AIAnimatorID.isRun, false);
    }

    private void StateChangeRunToAttack()
    {
        aiFSM.ChangeState(EAIState.Attack);
    }
}
