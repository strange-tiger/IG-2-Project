using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPun
{
    [SerializeField] GameObject _paintbrush;

    private PhotonView _photonView;
    private void Awake()
    {
        //Despawn();
        _photonView = _paintbrush.GetPhotonView();
    }

    private void OnDestroy()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        if (!_photonView.isRuntimeInstantiated)
        {
            return;
        }

        _photonView.RPC("Despawn", RpcTarget.All);
    }

    public void SpawnHelper()
    {
        if (!_photonView.IsMine)
        {
            return;
        }

        if (_photonView.isRuntimeInstantiated)
        {
            return;
        }

        _photonView.RPC("Spawn", RpcTarget.All);
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
        PhotonNetwork.Destroy(_paintbrush);
    }
}
