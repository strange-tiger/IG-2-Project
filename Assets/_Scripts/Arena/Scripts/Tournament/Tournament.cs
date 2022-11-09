using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tournament : MonoBehaviourPun
{
    [SerializeField] private float _reStartTime;

    private float _curTime;

    void Update()
    {
        _curTime += Time.deltaTime;
        if (_curTime >= _reStartTime)
        {
            Debug.Log("√ ±‚»≠");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            _curTime -= _curTime;
        }
    }
}
