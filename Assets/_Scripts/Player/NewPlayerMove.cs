using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerMove : MonoBehaviour
{
    private SwitchController _switchController;

    private bool _isControllerRight;

    private void Awake()
    {
        _switchController = GetComponentInChildren<SwitchController>();

        if (PlayerPrefs.HasKey("WhatIsTheMainController"))
        {
            _isControllerRight = Convert.ToBoolean(PlayerPrefs.GetInt("WhatIsTheMainController"));
        }
    }
    void Update()
    {
        if (_switchController.Type == 0)
        {
            PlayerRotate(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
        }
     
        else if (_switchController.Type != 0)
        {
            PlayerRotate(OVRInput.Touch.PrimaryThumbstick, OVRInput.Axis2D.PrimaryThumbstick);
        }
    }

    private void PlayerRotate(OVRInput.Touch value, OVRInput.Axis2D stick)
    {
        if (OVRInput.Get(value))
        {
            Vector2 thumbstick = OVRInput.Get(stick);

            if (thumbstick.x < 0)
            {
                gameObject.transform.Rotate(0, thumbstick.x, 0);
            }
            else if (thumbstick.x > 0)
            {
                gameObject.transform.Rotate(0, thumbstick.x, 0);
            }
        }
    }
}
