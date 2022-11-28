using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu3_Magic : MonoBehaviour
{
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) && _syncOVRDistanceGrabbable.isGrabbed)
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;

            enabled = false;
        }
    }
}
