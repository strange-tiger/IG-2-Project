using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoldBoxState : MonoBehaviourPun
{
    public void EnableScript(bool value)
    {
        photonView.RPC(nameof(EnableScriptByRPC), RpcTarget.All, value);
    }
    [PunRPC]
    protected virtual void EnableScriptByRPC(bool value)
    {
        this.enabled = value;
    }

    public void SetActiveObject(bool value)
    {
        photonView.RPC(nameof(SetActiveObjectByRPC), RpcTarget.All, value);
    }
    [PunRPC]
    protected virtual void SetActiveObjectByRPC(bool value)
    {
        gameObject.SetActive(value);
    }
}
