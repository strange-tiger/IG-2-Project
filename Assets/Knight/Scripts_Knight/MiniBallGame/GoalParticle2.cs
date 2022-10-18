using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalParticle2 : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        Invoke("SelfOff", 2f);
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
