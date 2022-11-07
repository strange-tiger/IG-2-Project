using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPaintbrush : MonoBehaviourPun, IPunObservable
{
    [SerializeField] GameObject _legalPad;
    [SerializeField] GameObject _pencil;

    private Transform _clientPlayer;
    
    public void SpawnHelper()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("Spawn", RpcTarget.AllBuffered);
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 1.5f, 1f);
    [PunRPC]
    private void Spawn()
    {
        transform.position = SPAWN_PAD_POSITION;
        _legalPad.SetActive(true);
        _pencil.SetActive(true);
    }
}
