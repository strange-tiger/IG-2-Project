//#define debugMaster
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ArenaPhotonTest : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _pos;

    private const string ROOM_NAME = "ArenaTest6";

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("�������� ��");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log("���� ���� ����");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
        Debug.Log("�������� ����");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("�κ����� ����");
#if debugMaster
        while (!PhotonNetwork.CreateRoom(ROOM_NAME)) ;
#else
        while (!PhotonNetwork.JoinRoom(ROOM_NAME));
#endif
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("�����");
        PhotonNetwork.LoadLevel("ArenaRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("�� ����");

        PhotonNetwork.Instantiate("NewPlayer", _pos.position, Quaternion.identity);
    }
}