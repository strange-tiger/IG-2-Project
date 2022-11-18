using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoldBoxServer : TestInPhoton
{
    [SerializeField] private GameObject _spawner;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if(PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(_spawner.name, Vector3.zero, Quaternion.identity);
        }
    }
}
