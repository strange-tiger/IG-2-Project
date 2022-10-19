using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticle2 : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("SelfOff", 3f);
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
