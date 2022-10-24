using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GoalInBall : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Collider _goalLine;

    //[SerializeField]
    //private GameObject _particle1Position;

    //[SerializeField]
    //private GameObject _particle2Position;

    [SerializeField]
    private GameObject _particle1;

    [SerializeField]
    private GameObject _particle2;

    private void OnTriggerEnter(Collider _goalLine)
    {
        if (_goalLine.gameObject.tag == "Ball" && _goalLine.gameObject.transform.position.y > gameObject.transform.position.y)
        {
            Debug.Log("Goal In!");
            photonView.RPC("StartParticle", RpcTarget.All);

            // PhotonNetwork.Instantiate("GoalParticle1", _particle1Position.transform.position, Quaternion.identity);
            // PhotonNetwork.Instantiate("GoalParticle2", _particle2Position.transform.position, Quaternion.identity);

            // Instantiate(_particle1, _particle1Position.transform.position, Quaternion.identity);
            // Instantiate(_particle2, _particle2Position.transform.position, Quaternion.identity);
        }
    }

    [PunRPC]
    public void StartParticle()
    {
        _particle1.SetActive(true);
        _particle2.SetActive(true);
    }

}
