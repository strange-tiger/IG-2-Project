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
    private const int DEFAULT_MAX_PLAYER = 8;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public static void JoinRandom()
    {
        try
        {
            _currentJoinRoom = CUSTOM_ROOM_PROPERTIES_UNLOCKED;
            PhotonNetwork.LeaveRoom();
        }
        catch
        {
            Debug.LogError("�κ� ���� ����");
        }
    }

    public static void JoinInRoom(_PH.Hashtable roomInfo)
    {
        try
        {
            _currentJoinRoom = roomInfo;
            PhotonNetwork.LeaveRoom();
        }
        catch
        {
            Debug.LogError("�κ� ���� ����");
        }
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        try
        {
            PhotonNetwork.JoinRandomOrCreateRoom(_currentJoinRoom, DEFAULT_MAX_PLAYER);
        }
        catch
        {
            Debug.LogError("�� ���� ����");
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        _DB.AddNewRoomInfo
        (
            _currentJoinRoom["roomname"].ToString(),
            _currentJoinRoom["password"].ToString(),
            _currentJoinRoom["displayname"].ToString(),
            DEFAULT_MAX_PLAYER
        );
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("[���� ��] " + PhotonNetwork.CurrentRoom.Name);

        OVRScreenFade.instance.FadeOut();

        PlayerPrefs.SetInt("PrevScene", SceneManagerHelper.ActiveSceneBuildIndex);
        PhotonNetwork.LoadLevel((int)Defines.ESceneNumber.PrivateRoom);
    }

}
