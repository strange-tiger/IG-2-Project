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

    [SerializeField] private OVRGrabber[] _grabbers;

    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
#if _PC_TEST
        if(!IsNearTumbleweed || !Input.GetKey(KeyCode.A))
#else
        if(!IsNearTumbleweed || !_input.InputA)
#endif
        {
            GrabbingTime = 0f;
            return;
        }

        foreach(OVRGrabber grabber in _grabbers)
        {
            if(grabber.grabbedObject)
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
