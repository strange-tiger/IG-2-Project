using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IsekaiObject : MonoBehaviourPun
{
    public event Action ObjectSlashed;

    private MeshRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("IsekaiWeapon"))
        {
            StartCoroutine(Flick());
        }
    }

    private IEnumerator Flick()
    {
        int count = 3;

        while (count > 0)
        {
            yield return null;
        }
    }
}
