using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPun
{
    [SerializeField] GameObject _legalPad;
    [SerializeField] GameObject _pencil;

    private Transform _clientPlayer;
    
    public void SetPlayerTransform(Transform player)
    {
        _clientPlayer = player;
    }

    public void TogglePaintbrush()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, "Spawn");
        PhotonNetwork.RemoveBufferedRPCs(photonView.ViewID, "Despawn");

        if (!_legalPad.activeSelf)
        {
            photonView.RPC("Spawn", RpcTarget.AllBuffered);
        }
        else
        {
            photonView.RPC("Despawn", RpcTarget.AllBuffered);
        }
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 0f, 1.5f);
    [PunRPC]
    private void Spawn()
    {
        if (photonView.IsMine)
        {
            transform.position = _clientPlayer.position + SPAWN_PAD_POSITION;
        }
        _legalPad.SetActive(true);
        _pencil.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        _legalPad.SetActive(false);
        _pencil.SetActive(false);
    }
}
