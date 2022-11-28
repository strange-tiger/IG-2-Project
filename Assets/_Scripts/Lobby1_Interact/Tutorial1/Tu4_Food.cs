using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class Tu4_Food : MonoBehaviour
{
    [SerializeField] private Lobby1TutorialsQuest _lobby1TutorialsQuest;
    private Outlinable _outlinable;

    private bool _isOutlinable;

    private void Update()
    {
        if (gameObject.GetComponent<Outlinable>() != null && !_isOutlinable)
        {
            _outlinable.GetComponent<Outlinable>();
            _isOutlinable = true;
        }
        else
        {
            _isOutlinable = false;
        }

        if (_isOutlinable)
        {
            if (OVRInput.GetDown(OVRInput.Button.One) && Input.GetKeyDown(KeyCode.O))
            {
                ++_lobby1TutorialsQuest.AdvanceQuest;
            }
        }
    }
}
