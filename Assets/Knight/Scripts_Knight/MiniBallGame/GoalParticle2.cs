using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticle2 : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Invoke("SelfDestroy", 2f);
    }

    private void SelfDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
