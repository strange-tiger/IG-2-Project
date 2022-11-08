using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EPetMoveState
{
    IDLE,
    MOVE,
    MAX
}

public class PetMove : MonoBehaviour
{
    public event Action<bool> OnStateChanged;
    public EPetMoveState State
    {
        get
        {
            return _state;
        }

        set
        {
            _state = value;
            OnStateChanged.Invoke(_state == EPetMoveState.MOVE);
        }
    }
    private EPetMoveState _state = EPetMoveState.IDLE;

    private PetManager _manager;
    private Animator _animator;
    private NavMeshAgent _agent;
    private Collider _senser;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _senser = GetComponentInParent<Collider>();
    }

    private void OnEnable()
    {
        State = EPetMoveState.IDLE;
        OnStateChanged -= ChangeMoveState;
        OnStateChanged += ChangeMoveState;
    }

    private void OnDisable()
    {
        OnStateChanged -= ChangeMoveState;
    }

    public void SetPetManager(PetManager manager)
    {
        _manager = manager;
    }

    private void ChangeMoveState(bool isMove)
    {
        _animator.SetBool("IsWalk", isMove);
        _agent.isStopped = !isMove;

        if (isMove)
        {
            _agent.SetDestination(_manager.transform.position);
        }
    }

}
