using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _paintbrush;

    private bool _isPaintbrushSpawned = false;

    private void Awake()
    {
        Despawn();
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 1.5f, 1f);
    [PunRPC]
    public void Spawn()
    {
        if (_isPaintbrushSpawned)
        {
            return;
        }

        _paintbrush.transform.position = SPAWN_PAD_POSITION;
        _paintbrush.SetActive(true);

    }

    [PunRPC]
    public void Despawn()
    {
        if (!_isPaintbrushSpawned)
        {
            return;
        }

        _paintbrush.SetActive(false);
    }
}
