using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSenser : MonoBehaviour
{
    private ExitWaiting _interaction;

    private void Awake()
    {
        _interaction = GetComponent<ExitWaiting>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _interaction.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _interaction.OutFocus();
        _interaction.enabled = false;
    }
}
