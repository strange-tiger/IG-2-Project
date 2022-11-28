using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu3_Magic : MonoBehaviour
{
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private bool _isFirst;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two) && _syncOVRDistanceGrabbable.isGrabbed && !_isFirst)
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;
            _isFirst = true;
        }
    }
}
