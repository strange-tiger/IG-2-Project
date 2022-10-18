using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GoalParticle1 : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Invoke("SelfOff", 1f);
    }

    private void SelfOff()
    {
        // PhotonNetwork.Destroy(gameObject);
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void StartParticle()
    {
        gameObject.SetActive(true);
    }

}
