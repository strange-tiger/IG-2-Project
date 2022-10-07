using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float InputX { get; private set; }
    public float InputZ { get; private set; }
    public bool isRay { get; private set; }

    void Update()
    {
        InputX = Input.GetAxis("Horizontal");
        InputZ = Input.GetAxis("Vertical");
        isRay = (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger));
    }
}
