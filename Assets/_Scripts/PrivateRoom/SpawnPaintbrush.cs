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
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_legalPad.activeSelf);
            stream.SendNext(_pencil.activeSelf);
        }
        else if (stream.IsReading)
        {
            _legalPad.SetActive((bool)stream.ReceiveNext());
            _pencil.SetActive((bool)stream.ReceiveNext());
        }
    }

    public void SpawnHelper()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        photonView.RPC("Spawn", RpcTarget.All);
    }

    private static readonly Vector3 SPAWN_PAD_POSITION = new Vector3(0f, 1.5f, 1f);
    [PunRPC]
    private void Spawn()
    {
        transform.position = SPAWN_PAD_POSITION;
        gameObject.SetActive(true);
    }
}
