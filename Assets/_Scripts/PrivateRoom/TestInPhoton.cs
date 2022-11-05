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
        Debug.Log("됐나?");
    }

    public override void OnJoinedLobby()
    {
        //base.OnJoinedLobby();
        try
        {

            Debug.Log("됐지?");
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        catch
        {
            Debug.LogError("방 입장 실패");
        }
    }

    public override void OnCreatedRoom()
    {
        //base.OnCreatedRoom();
        try
        {

            Debug.Log("됐구나?");
            PhotonNetwork.LoadLevel("ArenaRoom");
            Debug.Log(PhotonNetwork.CurrentRoom.Name);
        }
        catch
        {
            Debug.LogError("씬 로드 실패");
        }
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("된거구나?");
        PhotonNetwork.Instantiate("NewPlayer", Vector3.zero, Quaternion.identity);
    }
}
