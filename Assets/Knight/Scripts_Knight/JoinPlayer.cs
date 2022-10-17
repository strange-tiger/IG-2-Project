using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinPlayer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Invoke("SettingController", 1f);
    }

    private void SettingController()
    {
        PhotonNetwork.Instantiate("OVRPlayerController", Vector3.zero, Quaternion.identity);
        Debug.Log("¾å");
    }
}
