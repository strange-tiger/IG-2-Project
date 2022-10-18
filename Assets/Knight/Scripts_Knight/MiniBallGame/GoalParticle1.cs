using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GoalParticle1 : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Invoke("SelfDestroy", 1f);
    }

    private void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

}
