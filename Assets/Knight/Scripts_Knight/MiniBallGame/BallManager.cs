using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Vector3 _ballSspawnVector;

    void Start()
    {
        PhotonNetwork.Instantiate("Ball", _ballSspawnVector, Quaternion.identity);
    }
}
