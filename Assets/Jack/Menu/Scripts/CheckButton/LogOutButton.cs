using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LogOutButton : NeedCheckButton
{
    protected override void AcceptAction()
    {
        Debug.Log("LogOut");
        // PhotonNetwork.LoadLevel()
    }
}
