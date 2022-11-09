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

        while (!PhotonNetwork.JoinRandomOrCreateRoom());
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        
        Debug.Log("�Ʊ���?");
        PhotonNetwork.LoadLevel("Pet_Interaction");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("�Ȱű���?");

        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
