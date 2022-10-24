using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerDetector : MonoBehaviour
{
    Action<Collider> _onEnter;
    Action<Collider> _onStay;
    Action<Collider> _onExit;

    public void Init(Action<Collider> onEnter = null, Action<Collider> onStay = null, Action<Collider> onExit = null)
    {
        _onEnter = onEnter;
        _onStay = onStay;
        _onExit = onExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        _onEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        _onStay?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _onExit?.Invoke(other);
    }
}
