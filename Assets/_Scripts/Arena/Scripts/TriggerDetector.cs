using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private Collider _playerCollider;

    public UnityEvent _hiAI = new UnityEvent();
    public UnityEvent _attackAI = new UnityEvent();

    public void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            _attackAI.Invoke();
        }

        if (other.gameObject.tag == "AI")
        {
            _hiAI.Invoke();
            transform.LookAt(other.gameObject.transform);
            _playerCollider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }
}
