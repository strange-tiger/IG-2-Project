using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GoalInBall : MonoBehaviourPunCallbacks
{
    private ParticleSystem[] _goalParticle;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _goalParticle[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball" && other.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            photonView.RPC("GoalIn", RpcTarget.All);
        }
    }

    [PunRPC]
    public void GoalIn()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _goalParticle[i].gameObject.SetActive(true);
        }
    }
}
