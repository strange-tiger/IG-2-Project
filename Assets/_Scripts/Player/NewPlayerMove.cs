using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewPlayerMove : MonoBehaviour
{
    private SwitchController _switchController;

    private void Awake()
    {
        _switchController = GetComponentInChildren<SwitchController>();
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
