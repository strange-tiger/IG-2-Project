//#define debugMaster
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Asset.MySql;
public class TestInPhoton : MonoBehaviourPunCallbacks
{
    private const string ROOM_NAME = "PetTest";

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
#if debugMaster
        while (!PhotonNetwork.CreateRoom(ROOM_NAME)) ;
#else
        while (!PhotonNetwork.JoinRoom(ROOM_NAME)) ;
#endif
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("됐구나?");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("된거구나?");
        MySqlSetting.Init();

        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}