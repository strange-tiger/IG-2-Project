using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPun
{
    [SerializeField] GameObject _paintbrush;

    private void Awake()
    {
        Despawn();
    }

    private void OnDestroy()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!photonView.isRuntimeInstantiated)
        {
            return;
        }

        photonView.RPC("Despawn", RpcTarget.All);
    }

    public void SpawnHelper()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (photonView.isRuntimeInstantiated)
        {
            return;
        }

        photonView.RPC("Spawn", RpcTarget.All);
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 1.5f, 1f);
    [PunRPC]
    private void Spawn()
    {
        _paintbrush = PhotonNetwork.Instantiate("Paintbrush", transform.position + SPAWN_PAD_POSITION, transform.rotation);
    }

    [PunRPC]
    private void Despawn()
    {
        Destroy(_paintbrush);
    }
}
