using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class GoldBoxState : MonoBehaviourPunCallbacks
{
    protected bool _isJoinedRoom = false;

    public override void OnJoinedRoom()
    {
        _isJoinedRoom = true;
    }

    public abstract void EnableScript(bool value);

    public abstract void SetActiveObject(bool value);
}