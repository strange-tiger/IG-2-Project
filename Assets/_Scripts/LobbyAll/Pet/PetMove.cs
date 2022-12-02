using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using UnityEngine.SocialPlatforms;

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

    protected int _targetId;
    protected Transform _targetTransform;
    protected NavMeshAgent _agent;
    private Animator _animator;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        OnStateChanged -= ChangeMoveState;
        OnStateChanged += ChangeMoveState;

        State = EPetMoveState.IDLE;
    }

    private void OnDisable()
    {
        OnStateChanged -= ChangeMoveState;
    }

    protected const string PET_SPAWNER_TAG = "PetSpawner";
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PET_SPAWNER_TAG) && other.gameObject.GetPhotonView().ViewID == _targetId)
        {
            State = EPetMoveState.IDLE;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(PET_SPAWNER_TAG) && other.gameObject.GetPhotonView().ViewID == _targetId)
        {
            State = EPetMoveState.MOVE;
        }
    }

    public void SetTarget(int id, Transform target)
    {
        _targetId = id;
        _targetTransform = target;
    }

    protected const string PARAM_IS_WALK = "IsWalk";
    private void ChangeMoveState(bool isMove)
    {
        _animator.SetBool(PARAM_IS_WALK, isMove);

        ChangeMoveStateHelper(isMove);
    }

    protected void ChangeMoveStateHelper(bool isMove)
    {
        if (isMove)
        {
            StartCoroutine(TrackDestination());
        }

        _agent.isStopped = !isMove;
    }

    protected IEnumerator TrackDestination()
    {
        while(_state == EPetMoveState.MOVE)
        {
            _agent.SetDestination(_targetTransform.position);
            yield return null;
        }
    }
}