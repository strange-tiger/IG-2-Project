using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTumbleweedInteraction : MonoBehaviour
{
    public bool IsNearTumbleweed { get; set; }

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
        if(!IsNearTumbleweed || !_input.InputA)
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
}
