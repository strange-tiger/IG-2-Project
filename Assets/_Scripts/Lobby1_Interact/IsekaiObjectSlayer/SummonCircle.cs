using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SummonCircle : MonoBehaviourPun
{
    [SerializeField] GameObject[] _objects;

    private static readonly Vector3 FLOAT_POSITION = new Vector3(0f, 1.2f, 0f);

    private void OnEnable()
    {
        
    }
}
