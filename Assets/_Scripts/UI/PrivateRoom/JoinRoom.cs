using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

using _PH = ExitGames.Client.Photon;
using _DB = Asset.MySql.MySqlSetting;

public class JoinRoom : MonoBehaviourPunCallbacks
{
    private static _PH.Hashtable _currentJoinRoom = new _PH.Hashtable();
    private static readonly _PH.Hashtable CUSTOM_ROOM_PROPERTIES_UNLOCKED =
        new _PH.Hashtable() { { "password", "" } };
    private const int ANY_MAX_PLAYER = 0;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public static void JoinRandom()
    {
        try
        {
            _currentJoinRoom = CUSTOM_ROOM_PROPERTIES_UNLOCKED;
            PhotonNetwork.JoinLobby();
        }
        catch
        {
            Debug.LogError("로비 입장 실패");
        }
    }

    public static void JoinInRoom(_PH.Hashtable roomInfo)
    {
        try
        {
            _currentJoinRoom = roomInfo;
            PhotonNetwork.JoinLobby();
        }
        catch
        {
            Debug.LogError("로비 입장 실패");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        try
        {
            //PhotonNetwork.JoinRandomRoom(_currentJoinRoom, ANY_MAX_PLAYER);
            PhotonNetwork.JoinRandomOrCreateRoom();
        }
        catch
        {
            Debug.LogError("방 입장 실패");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        //_DB.AddNewRoomInfo("", "", "", 0);
        PhotonNetwork.LoadLevel("PrivateRoom_Interaction_Joker");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

}
