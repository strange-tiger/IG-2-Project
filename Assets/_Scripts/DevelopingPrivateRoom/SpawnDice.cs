using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPun
{
    [SerializeField] GameObject _dice;

    private PhotonView _photonView;
    private void Awake()
    {
        //if(!PhotonNetwork.IsMasterClient)
        //{
        //    return;
        //}
        Despawn();
        _photonView = _dice.GetPhotonView();
    }

    public void ToggleDice()
    {
        Debug.Log("spawn dice");
        if (!_photonView.isActiveAndEnabled)
        {
            _photonView.RPC("Spawn", RpcTarget.All);
        }
        else
        {
            _photonView.RPC("Despawn", RpcTarget.All);
        }
    }

    private static readonly Vector3 SPAWN_POSITION = new Vector3(0f, 1.5f, 1f);
    [PunRPC]
    private void Spawn()
    {
        _dice.transform.position = transform.position + SPAWN_POSITION;
        _dice.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        _dice.SetActive(false);
    }
}
