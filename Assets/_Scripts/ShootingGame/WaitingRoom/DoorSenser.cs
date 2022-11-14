using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSenser : MonoBehaviour
{
    [SerializeField] private GameObject _interactionObject;
    private WaitingRoomDoorInteraction _interaction;

    private void Awake()
    {
        _interaction = _interactionObject.GetComponent<WaitingRoomDoorInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PlayerBody"))
        {
            _interactionObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerBody"))
        {
            _interaction.OutFocus();
            _interactionObject.SetActive(false);
        }
    }
}
