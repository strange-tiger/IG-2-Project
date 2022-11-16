using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPun
{
    [SerializeField] GameObject _dice;

    private Transform _hostPlayer;

    public void SetPlayerTransform(Transform player)
    {
        _hostPlayer = player;
    }

    public void ToggleDice()
    {
        if (!_dice.activeSelf)
        {
            photonView.RPC("Spawn", RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("Despawn", RpcTarget.AllBuffered);
        }
    }

    private static readonly Vector3 SPAWN_POSITION = new Vector3(-1f, 2f, 4.8f);
    [PunRPC]
    private void Spawn()
    {
        if (photonView.IsMine)
        {
            transform.position = SPAWN_POSITION;
        }
        _dice.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        _dice.SetActive(false);
    }
}
