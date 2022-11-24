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
    public bool[] IsRays { get; private set; }
    public bool[] IsRayDowns { get; private set; }
    public bool IsLeftRay { get; private set; }
    public bool IsLeftRayDown { get; private set; }
    public bool IsRightRay { get; private set; }
    public bool IsRightRayDown { get; private set; }

    public bool IsMove { get; private set; }
    public bool IsInventoryOn { get; private set; }
    public bool IsInventoryOff { get; private set; }
    public bool IsGrab { get; private set; }
    public bool[] IsGrabs { get; private set; }
    public bool IsLeftGrab { get; private set; }
    public bool IsRightGrab { get; private set; }

    public Defines.EPrimaryController PrimaryController { get; private set; }

    private SwitchController _switchController;

    private void Awake()
    {
        _switchController = GetComponentInChildren<SwitchController>();
        if(_switchController)
        {
            _switchController.SwitchControllerEvent.RemoveListener(SetPrimaryController);
            _switchController.SwitchControllerEvent.AddListener(SetPrimaryController);
        }

        PrimaryController = Defines.EPrimaryController.Left;
        IsRays = new bool[2];
        IsRayDowns = new bool[2];
        IsGrabs = new bool[2];
    }

    private void SetPrimaryController(bool isRight)
    {
        PrimaryController = isRight ? 
            Defines.EPrimaryController.Right : Defines.EPrimaryController.Left;
    }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");

        InputADown = OVRInput.GetDown(OVRInput.Button.One);
        InputA = OVRInput.Get(OVRInput.Button.One);
    
        IsRays[0] = IsLeftRay = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
        IsRays[1] = IsRightRay = OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger);

        IsRayDowns[0] = IsLeftRayDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
        IsRayDowns[1] = IsRightRayDown = OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger);

        IsRay = IsLeftRay || IsRightRay;

        IsLeftGrab = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger);
        IsRightGrab = OVRInput.Get(OVRInput.Button.SecondaryHandTrigger);
        IsGrab = IsLeftGrab || IsRightGrab;
        IsGrabs[0] = IsLeftGrab;
        IsGrabs[1] = IsRightGrab;
        
        IsMove = OVRInput.Get(OVRInput.Touch.PrimaryThumbstick);
        
        IsInventoryOn = (OVRInput.GetDown(OVRInput.Button.Start)) || (Input.GetKeyDown(KeyCode.Y));
    }

    private void OnDisable()
    {
        if(_switchController)
        {
            _switchController.SwitchControllerEvent.RemoveListener(SetPrimaryController);
        }
    }
}
