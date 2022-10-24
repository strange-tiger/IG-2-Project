using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    public UnityEvent _onSword = new UnityEvent();
    public UnityEvent _onStay = new UnityEvent();
    public UnityEvent _onExit = new UnityEvent();

    public void Init()
    {
        Debug.Log("Init");
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            Debug.Log("À¸¾Ç!");
            _onSword.Invoke();
        }

        if (other.gameObject.tag == "AI")
        {
            Debug.Log("Àû¹ß°ß");
            _onExit.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        _onStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        _onExit.Invoke();
    }
}
