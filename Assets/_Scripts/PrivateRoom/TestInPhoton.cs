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

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("서버 연결 실패");
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
        
        Debug.Log("됐구나?");
        PhotonNetwork.LoadLevel("Pet_Interaction");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("된거구나?");

        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
