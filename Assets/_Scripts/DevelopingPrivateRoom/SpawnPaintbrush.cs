using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject _legalPad;
    [SerializeField] GameObject _pencil;

    private bool _isPaintbrushSpawned = false;

    private void Awake()
    {
        Despawn();
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 1.5f, 1f);
    private static readonly Quaternion SPAWN_ROTATION = Quaternion.Euler(-90f, 0f, 0f);
    private static readonly Vector3 SPAWN_PENCIL_POSITION = new Vector3(0.2f, 1.5f, 1f);
    [PunRPC]
    public void Spawn()
    {
        if (_isPaintbrushSpawned)
        {
            return;
        }

        _legalPad.transform.position = SPAWN_PAD_POSITION;
        _legalPad.transform.rotation = SPAWN_ROTATION;
        _legalPad.SetActive(true);

        _pencil.transform.position = SPAWN_PENCIL_POSITION;
        _pencil.transform.rotation = SPAWN_ROTATION;
        _pencil.SetActive(true);
    }

    [PunRPC]
    public void Despawn()
    {
        if (!_isPaintbrushSpawned)
        {
            return;
        }

        _legalPad.SetActive(false);
        _pencil.SetActive(false);
    }
}
