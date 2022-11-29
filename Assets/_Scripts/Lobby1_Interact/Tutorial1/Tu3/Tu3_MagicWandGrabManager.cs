using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu3_MagicWandGrabManager : MonoBehaviour
{
    [SerializeField]
    private Tu3_MagicWand _magicWand;

    private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;

    private bool _isReady;

    private void Start()
    {
        _magicWand.enabled = false;
        _syncOVRDistanceGrabbable = GetComponent<SyncOVRDistanceGrabbable>();
    }

    private void Update()
    {
        if (_syncOVRDistanceGrabbable.isGrabbed == true && !_isReady)
        {
            _magicWand.enabled = true;
            _isReady = true;
        }
        else if (_syncOVRDistanceGrabbable.isGrabbed == false && _isReady)
        {
            _magicWand.enabled = false;
            _isReady = false;
        }
    }
}
