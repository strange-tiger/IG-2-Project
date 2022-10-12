using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LogOutButton : NeedChangeButton
{
    protected override void AcceptAction()
    {
        Debug.Log("LogOut");
        // PhotonNetwork.LoadLevel()
    }
}
