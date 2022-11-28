using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu6_IsekaiCutting : MonoBehaviour
{
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IsekaiWeapon")
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;

            enabled = false;
        }
    }
}
