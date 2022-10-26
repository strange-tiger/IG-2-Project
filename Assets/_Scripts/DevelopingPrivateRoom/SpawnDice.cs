using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _dice;

    private bool _isDiceSpawned = false;

    private void Awake()
    {
        Despawn();
    }

    public void ToggleDice()
    {
        _isDiceSpawned = !_isDiceSpawned;

        if (_isDiceSpawned)
        {
            Spawn();
        }
        else
        {
            Despawn();
        }
    }

    private static readonly Vector3 SPAWN_POSITION = new Vector3(0f, 1.5f, 1f);
    [PunRPC]
    private void Spawn()
    {
        _dice.transform.position = SPAWN_POSITION;
        _dice.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        _dice.SetActive(false);
    }
}
