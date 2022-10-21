using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

using SceneType = Defines.ESceneNumder;

public class TestLogin : MonoBehaviourPunCallbacks
{


    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(SceneType.StartRoom.ToString(), new RoomOptions { }, TypedLobby.Default);

    }

}
