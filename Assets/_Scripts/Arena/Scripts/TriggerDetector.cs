using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private Collider _aiCollider;

    [SerializeField]
    private AIDamage _aIDamage;

    public UnityEvent HiAI = new UnityEvent();
    public UnityEvent AttackAI = new UnityEvent();

    public void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            AttackAI.Invoke();
        }

        if (other.gameObject.tag == "AI")
        {
            HiAI.Invoke();
            transform.LookAt(other.gameObject.transform);
            _aiCollider.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
