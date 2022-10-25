using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using EAIState = Defines.Estate;

public class TriggerDetector : AIState
{
    public void Init()
    {
        Debug.Log("Init");
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "AISword")
        //{
        //    aiFSM.ChangeState(EAIState.Damage);
        //    Debug.Log("칼맞음");
        //}

        if (other.gameObject.tag == "AI")
        {
            aiFSM.ChangeState(EAIState.Attack);
            Debug.Log("적발견");
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {

    }

    public override void OnEnter()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnExit()
    {
    }
}
