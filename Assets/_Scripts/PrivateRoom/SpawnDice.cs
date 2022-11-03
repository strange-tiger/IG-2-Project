using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPun
{
    [SerializeField] GameObject _dice;

    private Collider _diceCollider;
    private Transform _hostPlayer;

    private void Awake()
    {
        _diceCollider = GetComponent<Collider>();
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

    private static readonly Vector3 SPAWN_POSITION = new Vector3(0f, 1.5f, 5f);
    [PunRPC]
    private void Spawn()
    {
        transform.position = SPAWN_POSITION;
        _dice.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        _dice.SetActive(false);
    }
}
