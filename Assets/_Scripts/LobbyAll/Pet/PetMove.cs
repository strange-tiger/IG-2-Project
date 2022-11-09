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
    private EPetMoveState _state = EPetMoveState.IDLE;

    private PetSpawner _spawner;
    private Animator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        OnStateChanged -= ChangeMoveState;
        OnStateChanged += ChangeMoveState;
    }

    private void OnDisable()
    {
        OnStateChanged -= ChangeMoveState;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            State = EPetMoveState.IDLE;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && photonView.IsMine)
        {
            State = EPetMoveState.MOVE;
        }
    }

    public void SetPetSpawner(PetSpawner spawner)
    {
        _spawner = spawner;
    }

    private void ChangeMoveState(bool isMove)
    {
        _animator.SetBool("IsWalk", isMove);

        if (isMove)
        {
            _agent.SetDestination(_spawner.transform.position);
            Debug.Log(_spawner.transform.position);
        }

        _agent.isStopped = !isMove;
    }
}