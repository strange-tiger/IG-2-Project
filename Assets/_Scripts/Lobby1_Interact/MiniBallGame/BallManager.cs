using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Vector3 _ballSspawnVector;

    //[SerializeField]
    //private GameObject _ball;

    void Start()
    {
        PhotonNetwork.Instantiate("Ball", transform.position + _ballSspawnVector, Quaternion.identity);
        //Instantiate(_ball, _ballSspawnVector, Quaternion.identity);
    }
}
