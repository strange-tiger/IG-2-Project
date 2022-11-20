using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MagicWandGrabManager : MonoBehaviourPun
{
    [SerializeField]
    private MagicWand _magicWand;

    private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;

    private bool _isReady;

    private void Start()
    {
        if (photonView.IsMine)
        {
            _magicWand.enabled = false;
        }
        _syncOVRDistanceGrabbable = GetComponent<SyncOVRDistanceGrabbable>();
    }

    private void Update()
    {
        if (_syncOVRDistanceGrabbable.isGrabbed == true)
        {
            _magicWand.enabled = true;
        }
        else if (_syncOVRDistanceGrabbable.isGrabbed == false)
        {
            _magicWand.enabled = false;
        }
    }
}
