//#define _PC_TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTumbleweedInteraction : MonoBehaviour
{
    private bool _isNearTumbleweed;
    public bool IsNearTumbleweed 
    {
        get => _isNearTumbleweed;
        set
        {
            _isNearTumbleweed = value;
            if(!_isNearTumbleweed)
            {
                GrabbingTime = 0f;
            }
        }
    }

    private bool _isGrabbing;
    public float GrabbingTime { get; private set; }

    public SyncOVRGrabber[] Grabbers { get; set; }

    public PlayerInput Input { get; set; }

    private void FixedUpdate()
    {
#if _PC_TEST
        if(!IsNearTumbleweed || !Input.GetKey(KeyCode.A))
#else
        if(!IsNearTumbleweed || !Input.InputA)
#endif
        {
            GrabbingTime = 0f;
            return;
        }

        foreach (SyncOVRGrabber grabber in Grabbers)
        {
            if (grabber.grabbedObject)
            {
                GrabbingTime = 0f;
                return;
            }
        }

        GrabbingTime += Time.fixedDeltaTime;
    }

    public void GetGold(int gold)
    {
        Debug.Log("Gold ¹ÞÀ½ " + gold);
        IsNearTumbleweed = false;
    }
}
