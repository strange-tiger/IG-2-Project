using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Defines
{
    public enum EPrimaryController
    {
        Left,
        Right,
    }
}

public class PlayerInput : MonoBehaviourPun
{
    public float InputX { get; private set; }
    public float InputZ { get; private set; }

    public bool InputADown { get; private set; }
    public bool InputA { get; private set; }

    public bool IsRay { get; private set; }
    public bool IsLeftRay { get; private set; }
    public bool IsRightRay { get; private set; }

    public bool IsMove { get; private set; }
    public bool IsInventoryOn { get; private set; }

    public bool IsGrab { get; private set; }
    public bool IsLeftGrab { get; private set; }
    public bool IsRightGrab { get; private set; }

    public Defines.EPrimaryController PrimaryController { get; private set; }

    private void Awake()
    {
        PrimaryController = Defines.EPrimaryController.Left;
    }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        InputADown = OVRInput.GetDown(OVRInput.Button.One);
        InputA = OVRInput.Get(OVRInput.Button.One);
    
        IsLeftRay = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        IsRightRay = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);
        IsRay = IsLeftRay || IsRightRay;

        IsLeftGrab = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        IsRightGrab = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        IsGrab = IsLeftGrab || IsRightGrab;
        
        IsMove = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick);
        IsInventoryOn = (OVRInput.Get(OVRInput.Button.Start)) || (Input.GetKeyDown(KeyCode.Y));
    }

}
