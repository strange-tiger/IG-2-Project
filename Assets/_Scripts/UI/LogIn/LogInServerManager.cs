using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Asset.MySql;

using SceneType = Defines.ESceneNumder;

public class LogInServerManager : MonoBehaviourPunCallbacks
{
    void LogOut(string str)
    {
        Debug.Log("[Logout] " + str);
    }

    [SerializeField] private Button _loginButton;

    private void Awake()
    {
        LogOut("LogIn씬 Awake");
        _loginButton.interactable = PhotonNetwork.IsConnected;
        if(!PhotonNetwork.IsConnected)
        {
            Debug.LogError("서버 접속 중");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        LogOut("OnConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        LogOut("OnDisconnected");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        _loginButton.interactable = true;
    }

    public void LogIn()
    {
        LogOut("LogIn");
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsMoveable = false;

        if (MySqlSetting.CheckValueByBase(Asset.EaccountdbColumns.Nickname, TempAccountDB.Nickname, Asset.EaccountdbColumns.HaveCharacter, "1"))
        {
            SceneManager.LoadScene((int)SceneType.StartRoom);
        }
        else
        {
            SceneManager.LoadScene((int)SceneType.MakeCharacterRoom);
        }
    }
}
