//#define _PC_TEST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;

public class PlayerTumbleweedInteraction : PlayerInteractionSencer
{
    private bool _isNearTumbleweed;
    public bool IsNearTumbleweed 
    {
        get
        {
            return IsNearInteraction || _isNearTumbleweed;
        }
        set
        {
            _isNearTumbleweed = value;
            IsNearInteraction = value;
            if(!_isNearTumbleweed)
            {
                InteractingTime = 0f;
            }
        }
    }

    public float InteractingTime { get; private set; }

    private void FixedUpdate()
    {
#if _PC_TEST
        if(!IsNearTumbleweed || !Input.GetKey(KeyCode.A))
#else
        if(!IsNearTumbleweed || !Input.InputA)
#endif
        {
            InteractingTime = 0f;
            return;
        }

        //foreach (SyncOVRGrabber grabber in Grabbers)
        //{
        //    if (grabber.grabbedObject)
        //    {
        //        InteractingTime = 0f;
        //        return;
        //    }
        //}

        InteractingTime += Time.fixedDeltaTime;
    }

    public override void GetGold(int gold)
    {
        base.GetGold(gold);
        IsNearTumbleweed = false;
    }
}
