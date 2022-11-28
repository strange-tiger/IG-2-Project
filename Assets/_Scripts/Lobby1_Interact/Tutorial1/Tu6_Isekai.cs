using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu6_Isekai : MonoBehaviour
{
    [SerializeField] private SyncOVRDistanceGrabbable _syncOVRDistanceGrabbable;
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IsekaiQuest")
        {
            if (_syncOVRDistanceGrabbable.isGrabbed)
            {
                ++_lobby1TutorialsQuest.AdvanceQuest;

                enabled = false;
            }
        }
    }
}
