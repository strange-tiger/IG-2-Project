using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIHighClassAdventurerRun : AIState
{
    [SerializeField] private Collider _myCollider;

    private Animator _animator;

    private bool _changeStateRunToAttack;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerRun, true);
    }

    public override void OnUpdate()
    {
        if (_changeStateRunToAttack == true)
        {
            Debug.Log("asd");

            aiFSM.ChangeState(EAIState.Attack);

            _changeStateRunToAttack = false;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isHighClassAdventurerRun, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HighClassKnight")
        {
            _myCollider.enabled = false;
            _changeStateRunToAttack = true;
        }

        else if (other.gameObject.tag == "HighClassAdventurer")
        {
            _myCollider.enabled = false;
            _changeStateRunToAttack = true;

        }

        else if (other.gameObject.tag == "FireWizard")
        {
            _myCollider.enabled = false;
            _changeStateRunToAttack = true;
        }

        else if (other.gameObject.tag == "IceWizard")
        {
            _myCollider.enabled = false;
            _changeStateRunToAttack = true;
        }
    }
}
