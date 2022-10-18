using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInput : MonoBehaviourPun
{
    public float InputX { get; private set; }
    public float InputZ { get; private set; }
    public bool IsLeftRay { get; private set; }
    public bool IsRightRay { get; private set; }
    public bool IsMove { get; private set; }
    public bool IsInventoryOn { get; private set; }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        IsLeftRay = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        IsRightRay = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        IsMove = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick);
        IsInventoryOn = (OVRInput.Get(OVRInput.Button.Start)) || (Input.GetKeyDown(KeyCode.Y));
    }
}
