using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticle : MonoBehaviour
{
    [SerializeField]
    private Defines.EParticleInvokeTime _invokeWaitForSeconds;

    private void OnEnable()
    {
        Invoke("SelfOff", (float)_invokeWaitForSeconds);
    }

    private void SelfOff()
    {
        gameObject.SetActive(false);
    }
}
