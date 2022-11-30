using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum EPetMoveState
{
    IDLE,
    MOVE,
    MAX
}

public class PetMove : MonoBehaviourPun
{
    public event Action<bool> OnStateChanged;
    public EPetMoveState State
    {
        get { return _state; }

        set
        {
            _state = value;
            OnStateChanged.Invoke(_state == EPetMoveState.MOVE);
        }
    }
    protected EPetMoveState _state = EPetMoveState.IDLE;

    protected Transform _destination;
    protected NavMeshAgent _agent;
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        OnStateChanged -= ChangeMoveState;
        OnStateChanged += ChangeMoveState;
    }

    private void OnDisable()
    {
        OnStateChanged -= ChangeMoveState;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            State = EPetMoveState.IDLE;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            State = EPetMoveState.MOVE;
        }
    }

    public void SetTarget(Transform destination)
    {
        _destination = destination;
    }

    private void ChangeMoveState(bool isMove)
    {
        _animator.SetBool("IsWalk", isMove);

        ChangeMoveStateHelper(isMove);
    }

    protected void ChangeMoveStateHelper(bool isMove)
    {
        if (isMove)
        {
            _agent.SetDestination(_destination.position);
            Debug.Log(_destination.position);
        }

        _agent.isStopped = !isMove;
    }
}