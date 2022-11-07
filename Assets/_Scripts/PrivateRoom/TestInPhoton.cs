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
        Debug.Log("������");
    }

    private void OnDisconnectedFromMasterServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("���� ���� �ٽ� ���� ��");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("���������� �Ϸ� �κ� ���� ��");
    }

    private static readonly RoomOptions RandomRoomOptions = new RoomOptions()
    {
        MaxPlayers = 4
    };

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("�κ� ���� �Ϸ�");

        PhotonNetwork.JoinOrCreateRoom("aa", RandomRoomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log($"������ �� �̸� : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log("�� ����");

        PhotonNetwork.LoadLevel("ArenaRoom");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"������ �� �̸� : {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log("�� ����");
        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
