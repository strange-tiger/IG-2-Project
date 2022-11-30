using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetLODMove : PetMove
{
    private Animator[] _animators;

    private void Awake()
    {
        _animators = new Animator[transform.childCount];
        _animators = GetComponentsInChildren<Animator>();
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

    private void ChangeMoveState(bool isMove)
    {
        foreach (Animator animator in _animators)
        {
            animator.SetBool("IsWalk", isMove);
        }

        ChangeMoveStateHelper(isMove);
    }
}
