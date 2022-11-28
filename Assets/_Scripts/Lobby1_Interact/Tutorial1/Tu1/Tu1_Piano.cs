using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu1_Piano : MonoBehaviour
{
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("PlayerBody"))
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;

            enabled = false;
        }
    }
}
