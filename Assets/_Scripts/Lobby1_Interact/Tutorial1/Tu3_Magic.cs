using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu3_Magic : MonoBehaviour
{
    [SerializeField] private Tu3_MagicWand _tu3_MagicWand;
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) && _syncOVRDistanceGrabbable.isGrabbed)
        {
            ++_tu3_MagicWand.AdvanceQuest;

            enabled = false;
        }
    }
}
