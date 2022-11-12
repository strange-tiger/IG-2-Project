using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallManager : MonoBehaviourPun
{
    [SerializeField]
    private Vector3 _ballSspawnVector;

    //[SerializeField]
    //private GameObject _ball;

    void Start()
    {
        if(photonView.AmOwner)
        {
            PhotonNetwork.Instantiate("Ball", transform.position + _ballSspawnVector, Quaternion.identity);
        }
    }
}
