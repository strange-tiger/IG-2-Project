using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GoalInBall : MonoBehaviourPunCallbacks
{
    private ParticleSystem[] _goalIn;

    private void Awake()
    {
        _goalIn = new ParticleSystem[transform.childCount];

        for (int i = 0; i < transform.childCount; ++i)
        {
            _goalIn[i] = gameObject.transform.GetChild(i).GetComponentInChildren<ParticleSystem>();
        }
    }

    private void OnTriggerEnter(Collider _goalLine)
    {
        if (_goalLine.gameObject.tag == "Ball" && _goalLine.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            photonView.RPC("PlayGoalInParticle", RpcTarget.All);
        }
    }

    [PunRPC]
    public void PlayGoalInParticle()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            _goalIn[i].gameObject.SetActive(true);
        }
    }

}
