using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestStart : MonoBehaviourPunCallbacks
{
    private Defines.ESceneNumder _nextScene;
    private bool _needSceneChange = false;

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
        
            Debug.Log("[LogOut] LobbyChanger OnConnectedToMaster");
            PhotonNetwork.JoinLobby();
        
    }

    private RoomOptions _roomOptions = new RoomOptions
    {
        MaxPlayers = 30
    };
    public override void OnJoinedLobby()
    {
       
            PhotonNetwork.JoinOrCreateRoom(_nextScene.ToString(), _roomOptions, TypedLobby.Default);
        
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameObject player = PhotonNetwork.Instantiate("NewPlayer", new Vector3(0f, 1f, 3f), Quaternion.Euler(0f, 0f, 0f), 0, null);
    }
}
