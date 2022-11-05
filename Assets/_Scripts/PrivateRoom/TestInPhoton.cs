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

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        Debug.Log("�Ƴ�?");
    }

    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        try
        {

            Debug.Log("����?");
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        catch
        {
            Debug.LogError("�� ���� ����");
        }
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        try
        {

            Debug.Log("�Ʊ���?");
            PhotonNetwork.LoadLevel("ArenaRoom");
            Debug.Log(PhotonNetwork.CurrentRoom.Name);
        }
        catch
        {
            Debug.LogError("�� �ε� ����");
        }
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("�Ȱű���?");
        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
