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
        Debug.Log("���� ���� ����");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
        Debug.Log("�Ƴ�?");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("����?");
#if debugMaster
        while (!PhotonNetwork.CreateRoom(ROOM_NAME)) ;
#else
        while (!PhotonNetwork.JoinRoom(ROOM_NAME)) ;
#endif
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("�Ʊ���?");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("�Ȱű���?");
        MySqlSetting.Init();

        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}