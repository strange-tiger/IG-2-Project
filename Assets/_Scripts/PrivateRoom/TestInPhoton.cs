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
        Debug.Log("접속중");
    }

    private void OnDisconnectedFromMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("접속 실패 다시 접속 중");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("마스터접속 완료 로비 접속 중");
    }

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 4
    };

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("로비 접속 완료");

        PhotonNetwork.JoinOrCreateRoom("aa", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log($"생성한 방 이름 : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log("방 생성");

        PhotonNetwork.LoadLevel("ArenaRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"참가한 방 이름 : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log("룸 참가");
        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
