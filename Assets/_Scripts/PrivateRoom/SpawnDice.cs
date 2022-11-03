using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnDice : MonoBehaviourPun, IPunObservable
{
    [SerializeField] GameObject _dice;

    private Collider _diceCollider;
    private Transform _hostPlayer;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_dice.activeSelf);
            stream.SendNext(_diceCollider.enabled);
        }
        else if (stream.IsReading)
        {
            _dice.SetActive((bool)stream.ReceiveNext());
            _diceCollider.enabled = (bool)stream.ReceiveNext();
        }
    }

    private void Awake()
    {
        _diceCollider = GetComponent<Collider>();
    }

    public void ToggleDice()
    {
        if (!_dice.activeSelf)
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
        _dice.SetActive(true);
    }

    [PunRPC]
    private void Despawn()
    {
        _dice.SetActive(false);
    }
}
