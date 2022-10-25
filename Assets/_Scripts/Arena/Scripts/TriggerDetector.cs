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

    public UnityEvent _hiAI = new UnityEvent();
    public UnityEvent _attackAI = new UnityEvent();
    public UnityEvent _killAI = new UnityEvent();

    public void Init()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "AISword")
        {
            _attackAI.Invoke();
            
            if (other.gameObject.GetComponentInParent<AIDamage>().Hp <= other.gameObject.GetComponentInParent<AIDamage>()._damage)
            {
                Debug.Log("aa");
                _killAI.Invoke();
            }
            else
            {
                Debug.Log("bb");
            }
        }

        if (other.gameObject.tag == "AI")
        {
            _hiAI.Invoke();
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
