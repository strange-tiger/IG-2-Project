using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TestTest : MonoBehaviourPun
{
    [SerializeField] private float _reStartTime;

    private bool _isStart;
        
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !_isStart)
        {
            if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.G))
            {
                PhotonNetwork.Instantiate("Tournament", Vector3.zero, Quaternion.identity);
                _isStart = true;
            }
        }
    }
}
