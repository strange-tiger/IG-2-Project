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
    [SerializeField] private Button _loginButton;

    private void Awake()
    {
        _loginButton.interactable = PhotonNetwork.IsConnected;
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
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
        _loginButton.interactable = true;
    }

    public void LogIn()
    {
        PlayerControlManager.Instance.IsRayable = false;
        PlayerControlManager.Instance.IsMoveable = false;

        if (MySqlSetting.CheckValueByBase(Asset.EaccountdbColumns.Nickname, TempAccountDB.Nickname, Asset.EaccountdbColumns.HaveCharacter, "True"))
        {
            PhotonNetwork.NickName = TempAccountDB.Nickname;
            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 1);
            SceneManager.LoadScene((int)SceneType.StartRoom);
        }
        else
        {
            PhotonNetwork.NickName = TempAccountDB.Nickname;
            MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 1);
            SceneManager.LoadScene((int)SceneType.MakeCharacterRoom);
        }
    }
}
