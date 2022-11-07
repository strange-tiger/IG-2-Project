using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestInPhoton : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void OnDisconnectedFromMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("됐나?");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("됐지?");

        while (!PhotonNetwork.JoinRandomOrCreateRoom());
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
<<<<<<<<< Temporary merge branch 1
        
        Debug.Log("됐구나?");
        PhotonNetwork.LoadLevel("PrivateRoom_Interaction");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        
=========

        Debug.Log("됐구나?");
        PhotonNetwork.LoadLevel("PrivateRoom_Interaction");
        Debug.Log("된거구나?");

>>>>>>>>> Temporary merge branch 2
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("等芭备唱?");

        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
