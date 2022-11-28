using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu2_Ball : MonoBehaviour
{
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Ball"))
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;

            enabled = false;
        }
    }
}
