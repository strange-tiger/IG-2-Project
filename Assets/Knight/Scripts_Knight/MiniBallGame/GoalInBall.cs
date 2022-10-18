using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GoalInBall : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _particle1Position;

    [SerializeField]
    private GameObject _particle2Position;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            PhotonNetwork.Instantiate("GoalParticle1", _particle1Position.transform.position, Quaternion.identity);
            PhotonNetwork.Instantiate("GoalParticle2", _particle2Position.transform.position, Quaternion.identity);
        }
    }
}
