using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallManager : MonoBehaviourPun
{
    private Vector3 _ballSspawnVector;

    private bool _isball = true;

    private void Start()
    {
        _ballSspawnVector = new Vector3(22f, 6f, 8f);
    }

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && _isball)
        {
            PhotonNetwork.Instantiate("Ball", _ballSspawnVector, Quaternion.identity);

            _isball = false;
        }
    }
}
