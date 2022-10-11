using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonNetworkPractice : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Connected!");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"Fail... {cause}");
    }
}
