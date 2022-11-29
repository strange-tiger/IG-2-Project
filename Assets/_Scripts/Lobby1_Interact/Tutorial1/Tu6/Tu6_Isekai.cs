using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu6_Isekai : MonoBehaviour
{
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private bool _isFirst;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AI" && _syncOVRDistanceGrabbable.isGrabbed && !_isFirst)
        {
            if (_syncOVRDistanceGrabbable.isGrabbed)
            {
                ++_lobby1TutorialsQuest.AdvanceQuest;
                _isFirst = true;
            }
        }
    }
}
