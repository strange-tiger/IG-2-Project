using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPun, IPunObservable
{
    private Transform _hostPlayer;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.activeSelf);
        }
        else if (stream.IsReading)
        {
            gameObject.SetActive((bool)stream.ReceiveNext());
        }
    }

    public void ToggleDice()
    {
        if (!gameObject.activeSelf)
        {
            photonView.RPC("Spawn", RpcTarget.All);
        }
        else
        {
            photonView.RPC("Despawn", RpcTarget.All);
        }
    }

    private static readonly Vector3 SPAWN_POSITION = new Vector3(0f, 1.5f, 5f);
    [PunRPC]
    private void Spawn()
    {
        transform.position = SPAWN_POSITION;
        gameObject.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        gameObject.SetActive(false);
    }
}
