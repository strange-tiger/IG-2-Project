using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfirePlace : MonoBehaviour
{
    private const string CAMPFIRE_TAG = "Campfire";
    private const string STOP_COUNTDOWN = "StopCountDown";
    private const string START_COUNTDOWN = "StartCountDown";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            other.GetComponent<Wood>().photonView.RPC(STOP_COUNTDOWN, RpcTarget.All);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(CAMPFIRE_TAG))
        {
            other.GetComponent<Wood>().photonView.RPC(START_COUNTDOWN, RpcTarget.All);
        }
    }
}
