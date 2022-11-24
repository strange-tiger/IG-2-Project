using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class LogInPlayerMove : MonoBehaviour
{
    void Update()
    {
        PlayerRotate(OVRInput.Touch.SecondaryThumbstick, OVRInput.Axis2D.SecondaryThumbstick);
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
