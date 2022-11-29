using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tournament : MonoBehaviourPun
{
    [Header("Æ©Åä¸®¾ó ÇÁ¸®ÆÕ ÆÄ±«±îÁö °É¸®´Â ½Ã°£")]
    [SerializeField] private float _reStartTime;

    private float _curTime;

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _curTime += Time.deltaTime;

            if (_curTime >= _reStartTime)
            {
                PhotonNetwork.Destroy(gameObject);

                _curTime -= _curTime;
            }
        }
    }
}
