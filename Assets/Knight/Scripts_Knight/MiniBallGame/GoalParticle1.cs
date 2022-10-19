using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class GoalParticle1 : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("SelfOff", 2f);
    }

    private void Start()
    {
        
    }

    private void SelfOff()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        

    }



}
