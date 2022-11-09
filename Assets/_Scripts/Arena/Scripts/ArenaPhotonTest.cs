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
        Debug.Log("서버연결 중");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log("서버 연결 실패");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
        Debug.Log("서버연결 성공");
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("로비접속 성공");
#if debugMaster
        while (!PhotonNetwork.CreateRoom(ROOM_NAME)) ;
#else
        while (!PhotonNetwork.JoinRoom(ROOM_NAME));
#endif
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("룸생성");
        PhotonNetwork.LoadLevel("ArenaRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("룸 접속");

        PhotonNetwork.Instantiate("NewPlayer", _pos.position, Quaternion.identity);
    }
}