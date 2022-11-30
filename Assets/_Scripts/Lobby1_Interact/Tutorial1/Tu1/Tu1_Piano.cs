using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tu1_Piano : MonoBehaviour
{
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;

    private bool _isFirst;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("PlayerBody") && !_isFirst)
        {
            ++_lobby1TutorialsQuest.AdvanceQuest;
            _isFirst = true;
        }
    }

    private void OnEnable()
    {
        _isFirst = false;
    }
}
