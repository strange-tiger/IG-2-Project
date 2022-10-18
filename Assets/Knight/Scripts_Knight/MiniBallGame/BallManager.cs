using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallManager : MonoBehaviourPunCallbacks
{
    private Vector3 _ballSspawnVector;

    private void Awake()
    {
        _ballSspawnVector = Vector3.zero;
    }

    void Start()
    {
        PhotonNetwork.Instantiate("Ball", _ballSspawnVector, Quaternion.identity);
    }

    void Update()
    {
        
    }
}
