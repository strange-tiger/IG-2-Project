using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIRun : AIState
{
    private Animator _animator;

    [SerializeField]
    private TriggerDetector _triggerDetector;

    private float curTime;
    private bool isRunTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _triggerDetector = GetComponent<TriggerDetector>();
        _triggerDetector._onExit.AddListener(StopRun);

        isRunTime = true;
    }

    private void Update()
    {
        if (isRunTime)
        {
            curTime += Time.deltaTime;
        }

        if (curTime >= 3f)
        {
            int a = Random.Range(0, 361);
            transform.Rotate(new Vector3(0, a, 0));
            curTime -= curTime;
        }
    }

    public override void OnEnter()
    {
        Debug.Log("OnEnter AIRun");
        _animator.SetBool(AIAnimatorID.isRun, true);
    }

    public override void OnUpdate()
    {
        //aiFSM.ChangeState(EAIState.Attack);

        isRunTime = false;
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isRun, false);
        isRunTime = false;
    }

    private void StopRun()
    {
        _animator.SetBool(AIAnimatorID.isRun, false);
    }
}
