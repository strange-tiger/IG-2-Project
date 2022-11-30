using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Asset.MySql;
using Photon.Pun;

public class GameExitButton : NeedCheckButton
{
    protected override void AcceptAction()
    {
        Debug.Log("Game Exit");
        MySqlSetting.UpdateValueByBase(Asset.EaccountdbColumns.Nickname, PhotonNetwork.NickName, Asset.EaccountdbColumns.IsOnline, 0);
        Application.Quit();
    }
}
