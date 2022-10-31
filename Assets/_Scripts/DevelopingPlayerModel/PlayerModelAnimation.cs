using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public static class PlayerModelIK
{
    public static void SetIKPositionAndRotation(this Animator animator, AvatarIKGoal ikGoal, Transform targetTransform)
    {
        animator.SetIKPositionWeight(ikGoal, 1.0f);
        animator.SetIKRotationWeight(ikGoal, 1.0f);

        animator.SetIKPosition(ikGoal, targetTransform.position);
        animator.SetIKRotation(ikGoal, targetTransform.rotation);
    }
}

public class PlayerModelAnimation : MonoBehaviourPun
{
    [SerializeField] private Transform _leftHand;
    public Transform LeftHand { get => _leftHand; set => _leftHand = value; }

    [SerializeField] private Transform _rightHand;
    public Transform RightHand { get => _rightHand; set => _rightHand = value; }

    private Animator _animator;

    private void Awake()
    {
        if(photonView.IsMine)
        {
            _animator = GetComponent<Animator>();
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if(photonView.IsMine)
        {
            _animator.SetIKPositionAndRotation(AvatarIKGoal.LeftHand, LeftHand);
            _animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHand.rotation * Quaternion.Euler(0f, 0f, 90f));

            _animator.SetIKPositionAndRotation(AvatarIKGoal.RightHand, RightHand);
            _animator.SetIKRotation(AvatarIKGoal.RightHand, RightHand.rotation * Quaternion.Euler(0f, 0f, -90f));
        }
    }
}
