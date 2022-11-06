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
        Debug.Log("µÆ³ª?");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("µÆÁö?");

        while (!PhotonNetwork.JoinRandomOrCreateRoom());
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
        Debug.Log("µÆ±¸³ª?");
        PhotonNetwork.LoadLevel("PrivateRoom_Interaction");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("µÈ°Å±¸³ª?");

        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
