using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EAIState = Defines.Estate;

public class AIRun : AIState
{
    [Header("이동속도를 입력 해 주세요")]
    [SerializeField] private float _speed;

    private bool _changeStateRunToAttack;

    private void OnEnable()
    {
        _changeStateRunToAttack = false;
    }

    public override void OnEnter()
    {
        _animator.SetBool(AIAnimatorID.isRun, true);
    }

    public override void OnUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
        if (_changeStateRunToAttack == true)
        {
            aiFSM.ChangeState(EAIState.Attack);

            _changeStateRunToAttack = false;
        }
    }

    public override void OnExit()
    {
        _animator.SetBool(AIAnimatorID.isRun, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AIRange")
        {
            // _myCollider.enabled = false;
            _changeStateRunToAttack = true;
        }
    }
}
