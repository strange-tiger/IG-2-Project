using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseParticle : MonoBehaviour
{
    [SerializeField]
    private Defines.EParticleDurationTime _invokeWaitForSeconds;

    private Vector3 _transform;

    private void OnEnable()
    {
        _transform = gameObject.transform.position;
        Invoke("SelfOff", (float)_invokeWaitForSeconds);
    }

    private void Update()
    {
        gameObject.transform.position = _transform;
        gameObject.transform.rotation = Quaternion.identity;
    }

    private void SelfOff()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        gameObject.transform.position = GetComponentInParent<Transform>().position;
    }

}
