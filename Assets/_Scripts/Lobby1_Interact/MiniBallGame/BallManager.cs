using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BallManager : MonoBehaviourPun
{
    [SerializeField]
    private Vector3 _ballSspawnVector;

    private bool _isball = true;

    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && _isball)
        {
            PhotonNetwork.Instantiate("Ball", transform.position + _ballSspawnVector, Quaternion.identity);

            _isball = false;
        }
    }
}
