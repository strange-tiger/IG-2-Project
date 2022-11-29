using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu6_IsekaiCutting : MonoBehaviour
{
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private bool _isFirst;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "IsekaiWeapon" && !_isFirst)
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;
            _isFirst = true;
        }
    }
}
