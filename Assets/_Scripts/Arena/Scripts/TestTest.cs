using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestTest : MonoBehaviourPun
{
    private bool _isStart;

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !_isStart)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("¤·¤¾");
                PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
                _isStart = true;
            }
        }
    }
}
