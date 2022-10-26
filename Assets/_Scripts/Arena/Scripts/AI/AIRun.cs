using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIRun : AIState
{
    [SerializeField]
    private TriggerDetector _hiAIOne;

    [SerializeField]
    private TriggerDetector _hiAITwo;

    [SerializeField]
    private TriggerDetector _hiAIThree;

    private Animator _animator;
    
    private void OnEnable()
    {
        _animator = GetComponent<Animator>();

        _hiAIOne.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAIOne.HiAI.AddListener(StateChangeRunToAttack);

        _hiAITwo.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAITwo.HiAI.AddListener(StateChangeRunToAttack);

        _hiAIThree.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAIThree.HiAI.AddListener(StateChangeRunToAttack);
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isRun, true);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isRun, false);
    }

    private void StateChangeRunToAttack()
    {
        aiFSM.ChangeState(EAIState.Attack);
        Debug.Log("StateChangeRunToAttack");
    }

    private void OnDisable()
    {
        _hiAIOne.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAITwo.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAIThree.HiAI.RemoveListener(StateChangeRunToAttack);
    }
}
