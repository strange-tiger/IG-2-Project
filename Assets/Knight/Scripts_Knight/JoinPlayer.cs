using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class JoinPlayer : MonoBehaviourPunCallbacks
{
    private bool _isOn;

    private void Update()
    {
        if (!_isOn)
        {
            PhotonNetwork.Instantiate("OVRPlayerController", Vector3.zero, Quaternion.identity);
            Debug.Log("¾å");
            _isOn = true;
        }
    }
}
