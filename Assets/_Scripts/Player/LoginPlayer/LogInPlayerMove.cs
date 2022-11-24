using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LogInPlayerMove : MonoBehaviour
{
    private bool _isControllerRight;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("WhatIsTheMainController"))
        {
            _isControllerRight = Convert.ToBoolean(PlayerPrefs.GetInt("WhatIsTheMainController"));
        }
    }

    private void Update()
    {
        if(_isControllerRight)
        {
            PlayerRotate(OVRInput.Touch.PrimaryThumbstick, OVRInput.Axis2D.PrimaryThumbstick);
        }
        else
        {
            PlayerRotate(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
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
