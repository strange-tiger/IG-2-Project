using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestInPhoton : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform ffff;

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
    }

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 4
    };

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        PhotonNetwork.JoinOrCreateRoom("aa", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        PhotonNetwork.LoadLevel("ArenaRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate("NewPlayer", ffff.position, Quaternion.identity);
    }
}
