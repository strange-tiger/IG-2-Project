using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spinning : MonoBehaviourPun
{
    [SerializeField] private float rotateSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }
}
