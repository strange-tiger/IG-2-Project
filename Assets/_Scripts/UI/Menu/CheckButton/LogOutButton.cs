using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Asset.MySql;

using SeceneType = Defines.ESceneNumber;
using Photon.Realtime;

public class LogOutButton : NeedCheckButton
{
    [SerializeField] private bool _isStartRoom;
    protected override void AcceptAction()
    {
        Debug.Log("[LogOut] LogOut");
        Debug.Log("[LogOut] Is In Room? " + PhotonNetwork.InRoom);
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);
        if (_isStartRoom)
        {
            SceneManager.LoadScene((int)SeceneType.LogIn);
        }
        else
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("[LogOut] 룸 나감");
        SceneManager.LoadScene((int)SeceneType.LogIn);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[LogOut] 로비 참가함");
    }
}
