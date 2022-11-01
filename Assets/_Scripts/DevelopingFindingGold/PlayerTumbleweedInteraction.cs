using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTumbleweedInteraction : MonoBehaviour
{
    public bool IsNearTumbleweed { get; set; }

    private bool _isGrabbing;
    public float GrabbingTime { get; private set; }

    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if(IsNearTumbleweed)
        {
            _isGrabbing = _input.PrimaryController == Defines.EPrimaryController.Left ?
                    _input.IsLeftGrab : _input.IsRightGrab;

            if (_isGrabbing)
            {
                GrabbingTime += Time.fixedDeltaTime;
            }
            else
            {
                GrabbingTime = 0f;
            }
        }
        else
        {
            GrabbingTime = 0f;
        }
    }
}
