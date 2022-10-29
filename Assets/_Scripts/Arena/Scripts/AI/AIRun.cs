using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;
using EJobClass = Defines.EJobClass;

public class AIRun : AIState
{
    [SerializeField]
    private TriggerDetector _hiAIOne;

    [SerializeField]
    private TriggerDetector _hiAITwo;

    [SerializeField]
    private TriggerDetector _hiAIThree;

    private Animator _animator;
    private AI _ai;

    private void Awake()
    {
        _ai = GetComponent<AI>();
    }

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
        Debug.Log(_ai.ClassNumber);
        switch (_ai.ClassNumber)
        {
            case 0:
                _animator.SetBool(AIAnimatorID.isRun, true);
                break;
            case 1:
                _animator.SetBool(AIAnimatorID.isHighClassKnightRun, true);
                break;
            case 2:
                _animator.SetBool(AIAnimatorID.isHighClassAdventurerRun, true);
                break;
            case 3:
                _animator.SetBool(AIAnimatorID.isFireWizardRun, true);
                break;
            case 4:
                _animator.SetBool(AIAnimatorID.isIceWizardRun, true);
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            default:
                break;
        }
        
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {
        switch (_ai.ClassNumber)
        {
            case 0:
                _animator.SetBool(AIAnimatorID.isRun, true);
                break;
            case 1:
                _animator.SetBool(AIAnimatorID.isHighClassKnightRun, true);
                break;
            case 2:
                _animator.SetBool(AIAnimatorID.isHighClassAdventurerRun, true);
                break;
            case 3:
                _animator.SetBool(AIAnimatorID.isFireWizardRun, true);
                break;
            case 4:
                _animator.SetBool(AIAnimatorID.isIceWizardRun, true);
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            default:
                break;
        }
    }

    private void StateChangeRunToAttack()
    {
        aiFSM.ChangeState(EAIState.Attack);
    }

    private void OnDisable()
    {
        _hiAIOne.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAITwo.HiAI.RemoveListener(StateChangeRunToAttack);
        _hiAIThree.HiAI.RemoveListener(StateChangeRunToAttack);
    }
}
