using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float InputX { get; private set; }
    public float InputZ { get; private set; }
    public bool IsRay { get; private set; }
    public bool IsMove { get; private set; }
    public bool IsRotate { get; private set; }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        IsRay = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        IsMove = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick);
        IsRotate = OVRInput.Get(OVRInput.Touch.SecondaryThumbstick);
    }
}
